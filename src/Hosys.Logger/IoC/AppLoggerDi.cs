using Hosys.Logger.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Hosys.Logger.IoC;

public static class AppLoggerDi
{
    public static void AddAppLogger(this IServiceCollection services, LogConfigurationHandler handler)
    {
        // Create a new instance of LogConfiguration
        var configuration = new LogConfiguration();
        handler(configuration);

        // Add the configuration to the services
        services.AddSingleton(configuration);
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
    }
}
