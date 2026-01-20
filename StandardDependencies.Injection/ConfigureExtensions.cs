using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StandardDependencies.Models;

namespace StandardDependencies.Injection;

public static class ConfigureExtensions
{
    /// <summary>
    /// Configure common elements for the application, such as environment variables, OpenTelemetry and Swagger.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="openTelemetryOptions"></param>
    /// <param name="swaggerOptions"></param>
    public static void ConfigureCommonElements(this IHostApplicationBuilder builder,
        OpenTelemetryOptions? openTelemetryOptions = null,
        SwaggerOptions? swaggerOptions = null)
    {
        builder.Configuration.AddEnvironmentVariables();
        builder.ConfigureOpenTelemetry(openTelemetryOptions);
        builder.ConfigureSwagger(swaggerOptions);
    }
}