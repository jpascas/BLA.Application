using Application.Commands;
using Application.Handlers;
using Application.Queries;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;        

        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient<ICommandHandler<CreateUserCommand>, CreateUserHandler>();
        services.AddTransient<IUserQueryService, UserQueryService>();
        return services;
    }
}
