namespace StandardDependencies.Models;

public sealed class OpenTelemetryOptions
{
    public const string SectionName = "OpenTelemetry";

    public string ServiceName { get; set; } = string.Empty;
    public string Url { get; set; } = "http://localhost:4317";
    public string ServiceVersion { get; set; } = string.Empty;
    
    public List<ExporterTypes> Exporters { get; set; } = [ExporterTypes.OTLP];
}

public enum ExporterTypes
{
    OTLP,
    Console
}