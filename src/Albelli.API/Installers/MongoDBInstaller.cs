using Albelli.Core;
using Albelli.Core.Models.MongoEntities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Albelli.API.Installers;

public class MongoDBInstaller : IInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
    {
        services.AddSingleton<AlbelliContext>();
        services.Configure<MongoSettings>(options =>
        {
            options.ConnectionString
                = configuration.GetSection("MongoConnection:ConnectionString").Value;
            options.DatabaseName
                = configuration.GetSection("MongoConnection:Database").Value;
        });

        services.AddSingleton<IMongoClient>(c =>
        {
            IOptions<MongoSettings> settings = c.GetRequiredService<IOptions<MongoSettings>>();
            MongoClientSettings dbSettings = MongoClientSettings.FromConnectionString(settings.Value.ConnectionString);
            dbSettings.SslSettings.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
            return new MongoClient(dbSettings);
        });
    }
}
