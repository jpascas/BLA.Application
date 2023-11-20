using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Presentation.Mapping;
using Presentation.Middlewares;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using Application.Abstractions;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);
        services.AddControllers();        
        services.AddAutoMapper(c => c.AddProfile<AutoMappingProfile>());
        services.AddFluentValidationAutoValidation(configuration =>
        {
            // Disable the built-in .NET model (data annotations) validation.
            configuration.DisableBuiltInModelValidation = true;

            // Only validate controllers decorated with the `FluentValidationAutoValidation` attribute.
            configuration.ValidationStrategy = ValidationStrategy.Annotations;

            // Enable validation for parameters bound from `BindingSource.Body` binding sources.
            configuration.EnableBodyBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from `BindingSource.Form` binding sources.
            configuration.EnableFormBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from `BindingSource.Query` binding sources.
            configuration.EnableQueryBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from `BindingSource.Path` binding sources.
            configuration.EnablePathBindingSourceAutomaticValidation = true;

            // Enable validation for parameters bound from 'BindingSource.Custom' binding sources.
            configuration.EnableCustomBindingSourceAutomaticValidation = true;

            // Replace the default result factory with a custom implementation.
            configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });
        services.AddHttpContextAccessor();

        return services;
    }
}