namespace Albelli.API.Installers;

public static class InstallerExtensions
{
    public static void InstallServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        var installers = typeof(Program).Assembly.ExportedTypes
            .Where(m => typeof(IInstaller).IsAssignableFrom(m) && !m.IsInterface && !m.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>()
            .ToList();

        installers.ForEach(m => m.Install(services, configuration, webHostEnvironment));
    }
}