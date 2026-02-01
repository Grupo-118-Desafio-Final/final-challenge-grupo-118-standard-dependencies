using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace FirstApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RainController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RainController> _logger;

    public RainController(IHttpClientFactory httpClientFactory, ILogger<RainController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpGet(Name = "GetIfWillRain")]
    public async Task<bool> GetIfWillRain()
    {
        var tracer = TracerProvider.Default.GetTracer("FirstApi");

        using var span = tracer.StartActiveSpan("GetIfWillRain Operation");
        span.SetAttribute("operation.name", "GetIfWillRain");
        span.SetAttribute("foo", "bar");

        _logger.LogInformation("Ready to check if it will rain...");

        using (var httpClient = _httpClientFactory.CreateClient("FirstApi"))
        {
            try
            {
                var response = await httpClient.GetAsync("/WeatherForecast");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully called WeatherForecast API.");
                    span.SetStatus(Status.Ok);
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error calling WeatherForecast API.");
                span.SetStatus(Status.Error);
                return false;
            }
        }

        span.SetStatus(Status.Error);
        return false;
    }
}