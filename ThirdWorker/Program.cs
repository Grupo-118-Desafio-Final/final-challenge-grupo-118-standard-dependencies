using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StandardDependencies.Injection;
using StandardDependencies.Models;

namespace ThirdWorker;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        
        // Configurar OpenTelemetry usando ConfigureCommonElements
        var openTelemetryOptions = new OpenTelemetryOptions
        {
            ServiceName = "ThirdWorker",
            ServiceVersion = "1.0.0",
            Url = "https://localhost:4318",
            Exporters = new List<ExporterTypes> { ExporterTypes.Console }
        };

        builder.ConfigureCommonElements(openTelemetryOptions);

        // Registrar serviços customizados com injeção de dependência
        builder.Services.AddSingleton<IGreetingService, GreetingService>();
        builder.Services.AddHostedService<WorkerService>();

        var host = builder.Build();
        
        await host.RunAsync();
    }
}

