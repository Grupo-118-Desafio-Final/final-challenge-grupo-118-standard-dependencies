namespace StandardDependencies.Models;

public class SwaggerOptions
{
    public const string SectionName = "Swagger";
    
    public string Version { get; set; } = "v1";
    public string Title { get; set; } = "API";
    public string Description { get; set; } = "API Documentation";
    
    public string ContactName { get; set; } = "API Support";
    
    public string ContactUrl { get; set; } = "http://example.com/support";
}