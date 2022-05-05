namespace Albelli.API.Installers;

public class RedisCacheInstaller : IInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        var connectionString = configuration.GetSection("ConnectionStrings:RedisCacheConfig").Value;

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
        });
    }
}