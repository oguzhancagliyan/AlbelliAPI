namespace Albelli.API.Installers;

public interface IInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment);
}