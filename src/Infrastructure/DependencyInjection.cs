using Application;
using Domain.Repositories;
using Infrastructure.Bus;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistency.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        BLADBConnectionConfig ConnectionConfig = new BLADBConnectionConfig(config.GetConnectionString("BLA"));        
        services.AddSingleton<BLADBConnectionConfig>(ConnectionConfig);        
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ICommandBus, InmediateExecutionCommandBus>();
        return services;
    }
}
