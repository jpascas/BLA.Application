using Application;
using Domain.Repositories;
using Infrastructure.Bus;
using Infrastructure.Identity;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistency.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Application.Abstractions;
using Infrastructure.Identity.JWT;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Identity.Context;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        BLADBConnectionConfig ConnectionConfig = new BLADBConnectionConfig(config.GetConnectionString("BLA"));        
        services.AddSingleton<BLADBConnectionConfig>(ConnectionConfig);        
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ICommandBus, InmediateExecutionCommandBus>();

        services.AddTransient<IJwtProvider, JwtProvider>();
        services.Configure<PasswordHasherOptions>(o => o.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3);
        services.AddTransient<IPasswordHasher<User>, UserPasswordHasher>();
        services.AddTransient<IPasswordHasher, UserPasswordHasher>();
        
        services.AddTransient<IContextProvider, ContextProvider>();

        return services;
    }
}
