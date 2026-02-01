using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace FirstApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    private readonly Meter _meter;
    private readonly Counter<long> _apiCallsCounter;
    private readonly Histogram<double> _apiResponseTime;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;

        _meter = new Meter("FirstApi", "1.0.0");
        _apiCallsCounter =
            _meter.CreateCounter<long>("first.potato.calls.count", description: "Number of API calls made");
        _apiResponseTime =
            _meter.CreateHistogram<double>("first.potato.response.time", unit: "ms", description: "API response times");
    }


    [HttpGet(Name = "/GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var tracer = TracerProvider.Default.GetTracer("FirstApi");

        using var span = tracer.StartActiveSpan("GetWeatherForecast Operation");
        span.SetAttribute("operation.name", "GetWeatherForecast");

        _logger.LogInformation("Preparando requisição de WeatherForecast");

        long startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        Thread.Sleep(100);

        var weathers = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        _apiCallsCounter.Add(1, new KeyValuePair<string, object?>("endpoint", "/GetWeatherForecast"));

        long endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        _apiResponseTime.Record(endTime - startTime,
            new KeyValuePair<string, object?>("endpoint", "/GetWeatherForecast"));

        _logger.LogInformation("Total de previsões retornadas: {Count}", weathers.Length);

        span.SetStatus(Status.Ok);
        return weathers;
    }
}