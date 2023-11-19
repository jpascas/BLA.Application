using Application;
using Infrastructure;
using Microsoft.OpenApi.Models;
using Presentation;

namespace BLA.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true)
                .AddEnvironmentVariables() // this is important
                .Build();

            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddPresentation(); // contains  services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BLA API",
                    Description = "An ASP.NET Core Web API for Ballast Lane Applications",                    
                    Contact = new OpenApiContact
                    {
                        Name = "Jhonatan Passalacqua",
                        Email = "jpascas@gmail.com"                        
                    }
                });
            });

            var app = builder.Build();

            app.UseRouting();

            // app.UseAuthentication();
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                    {
                        // options.SwaggerEndpoint("/api-docs/v1/swagger.json", "BLA API v1");
                        //options.RoutePrefix = "api-docs";
                    });
            }

            // app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
