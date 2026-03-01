using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ThirdWorker;

// Exemplo de Worker Service que usa injeção de dependência
public class WorkerService : BackgroundService
{
    private readonly ILogger<WorkerService> _logger;
    private readonly IGreetingService _greetingService;

    public WorkerService(ILogger<WorkerService> logger, IGreetingService greetingService)
    {
        _logger = logger;
        _greetingService = greetingService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker starting at: {time}", DateTimeOffset.Now);

        var greeting = _greetingService.GetGreeting();
        _logger.LogInformation(greeting);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(2000, stoppingToken);
        }

        _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
    }
}