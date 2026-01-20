using StandardDependencies.Injection;
using StandardDependencies.Models;

namespace StandardDependencies.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Reading the configurations        
        var swaggerOptions = builder
            .Configuration
            .GetSection(SwaggerOptions.SectionName)
            .Get<SwaggerOptions>();

        var openTelemetryOptions = builder
            .Configuration
            .GetSection(OpenTelemetryOptions.SectionName)
            .Get<OpenTelemetryOptions>();

        // Add services to the container.
        builder.Services.AddControllers();
        
        // Using the lib
        builder.ConfigureCommonElements(openTelemetryOptions, swaggerOptions);

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        // Using the lib
        app.UseStandarizedSwagger(swaggerOptions);

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}