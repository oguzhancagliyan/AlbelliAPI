using Albelli.Core.Models.MongoEntities;
using AlbelliAPI.FunctionalTest.SeedData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Serilog;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace AlbelliAPI.FunctionalTest.Fixtures;

public class TestServerFixture : WebApplicationFactory<Program>
{
    public Mock<HttpClient> MockHttpClient;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureAppConfiguration(builder =>
                {
                    var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                    builder.AddJsonFile(configPath);
                })
                .ConfigureServices(collection =>
            {
                ServiceProvider serviceProvider = collection.BuildServiceProvider();

                var hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
                bool isTest = hostEnvironment.IsEnvironment("Testing");

                if (!isTest)
                {
                    throw new Exception("Incorrect config loaded.");
                }

                using var scope = serviceProvider.CreateScope();
                var scopeServiceProvider = scope.ServiceProvider;

                var context = scopeServiceProvider.GetRequiredService<AlbelliContext>();

                Seeder.Seed(context);

                collection.Remove(new ServiceDescriptor(typeof(IDistributedCache),
                    a => a.GetService(typeof(IDistributedCache)), ServiceLifetime.Singleton));

                var configuration = scopeServiceProvider.GetRequiredService<IConfiguration>();

                var connectionString = configuration.GetSection("ConnectionStrings:RedisCacheConfig").Value;

                collection.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = connectionString;
                });

                var mockHttpClient = new Mock<HttpClient>();
                MockHttpClient = mockHttpClient;

                var mockFactory = new Mock<IHttpClientFactory>();
                mockFactory.Setup(d => d.CreateClient(It.IsAny<string>()))
                    .Returns(mockHttpClient.Object);

                mockHttpClient
                    .Setup(d => d.SendAsync(It.IsAny<HttpRequestMessage>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new HttpResponseMessage
                    { StatusCode = HttpStatusCode.OK, Content = new StringContent("selam") });

                collection.Remove(new ServiceDescriptor(typeof(IHttpClientFactory),
                    a => a.GetService(typeof(IHttpClientFactory)), ServiceLifetime.Scoped));
                collection.AddScoped<IHttpClientFactory>(c => mockFactory.Object);


                collection.RemoveAll(typeof(IHostedService));
            })
            .UseEnvironment("Testing")
            .UseSerilog((context, loggerConfiguration) => { loggerConfiguration.WriteTo.Console(); });
    }
}