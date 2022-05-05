using Albelli.Core.Models.MongoEntities;
using AlbelliAPI.FunctionalTest.Fixtures;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http;

namespace Funda.FunctionalTests.Scenarios;

public class BaseScenario
{
    protected internal readonly TestServer TestServer;

    protected internal readonly HttpClient HttpClient;

    protected internal Mock<HttpClient> MockHttpClient;

    private TestServerFixture TestServerFixture;

    internal readonly AlbelliContext DbContext;

    protected BaseScenario(TestServerFixture testServerFixture)
    {
        TestServerFixture = testServerFixture;
        TestServer = testServerFixture.Server;
        HttpClient = testServerFixture.CreateClient();
        DbContext = TestServer.Services.GetRequiredService<AlbelliContext>();
        MockHttpClient = TestServerFixture.MockHttpClient;
    }
}