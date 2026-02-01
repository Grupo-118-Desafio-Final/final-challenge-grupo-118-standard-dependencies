using System.Reflection;
using StandardDependencies.Injection;
using StandardDependencies.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddHttpClient("FirstApi", client => { client.BaseAddress = new Uri("http://localhost:5277"); });

// ARTIGO BASE: https://medium.com/@faulycoelho/implementing-observability-in-a-net-applications-logging-tracing-and-metrics-67fe5b58312d

// Reading the configurations        
var swaggerOptions = builder
    .Configuration
    .GetSection(SwaggerOptions.SectionName)
    .Get<SwaggerOptions>();

var openTelemetryOptions = builder
    .Configuration
    .GetSection(OpenTelemetryOptions.SectionName)
    .Get<OpenTelemetryOptions>();

// Using the lib
builder.ConfigureCommonElements(openTelemetryOptions, swaggerOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Log: OK
// Metrics: OK
// Traces: Not ok 