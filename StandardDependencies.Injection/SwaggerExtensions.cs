using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using StandardDependencies.Models;

namespace StandardDependencies.Injection;

public static class SwaggerExtensions
{
    /// <summary>
    /// Configure Swagger services for API documentation. If necessary, the AddSwaggerGen method can be called again to add more configurations.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="swaggerOptions"></param>
    /// <exception cref="ArgumentNullException"></exception>
    internal static void ConfigureSwagger(this IHostApplicationBuilder builder, SwaggerOptions? swaggerOptions = null)
    {
        if (swaggerOptions == null)
            throw new ArgumentNullException(nameof(swaggerOptions));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            options.SwaggerDoc(swaggerOptions.Version, new OpenApiInfo
            {
                Title = swaggerOptions.Title,
                Version = swaggerOptions.Version,
                Description =
                    swaggerOptions.Description,
                Contact = new OpenApiContact
                {
                    Name = swaggerOptions.ContactName,
                    Url = new Uri(swaggerOptions.ContactUrl)
                }
            });
        });
    }

    /// <summary>
    /// Invoke Swagger middleware to serve generated Swagger as a JSON endpoint and the Swagger UI.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="swaggerOptions"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void UseStandarizedSwagger(this IApplicationBuilder app, SwaggerOptions? swaggerOptions = null)
    {
        if (swaggerOptions == null)
            throw new ArgumentNullException(nameof(swaggerOptions));

        app.UseSwagger();
        app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("../swagger/v1/swagger.json", swaggerOptions.Title);
                s.RoutePrefix = string.Empty;
                s.DocumentTitle = swaggerOptions.Title;
            }
        );
    }
}