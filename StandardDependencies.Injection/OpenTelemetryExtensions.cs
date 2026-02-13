using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StandardDependencies.Models;

namespace StandardDependencies.Injection;

public static class OpenTelemetryExtensions
{
    /// <summary>
    /// Configure the OpenTelemetry services to allow logging, tracing and metrics be sent to the desired exporters.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="openTelemetryOptions"></param>
    /// <exception cref="ArgumentNullException"></exception>
    internal static void ConfigureOpenTelemetry(this IHostApplicationBuilder builder,
        OpenTelemetryOptions? openTelemetryOptions = null)
    {
        if (openTelemetryOptions == null)
            throw new ArgumentNullException(nameof(openTelemetryOptions));

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(
                serviceName: openTelemetryOptions.ServiceName,
                serviceVersion: openTelemetryOptions.ServiceVersion,
                serviceInstanceId: Environment.MachineName)
            .AddAttributes(new[]
            {
                new KeyValuePair<string, object>("deployment.environment", environment),
            });

        // Configuração do OpenTelemetry Logging
        builder.Logging.ClearProviders();
        builder.Logging.AddJsonConsole(options =>
        {
            options.JsonWriterOptions = new JsonWriterOptions
            {
                Indented = false,
            };
        });
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.SetResourceBuilder(resourceBuilder);
            logging.IncludeFormattedMessage = false;
            logging.IncludeScopes = true;
            logging.ParseStateValues = true;
            logging.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri($"{openTelemetryOptions.Url}/v1/logs");
                options.Protocol = OtlpExportProtocol.HttpProtobuf;
            });
        });

        // Configuração do que será exportado no uso dos logs
        builder.Logging.Configure(options =>
        {
            options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                              | ActivityTrackingOptions.TraceId
                                              | ActivityTrackingOptions.ParentId
                                              | ActivityTrackingOptions.Baggage
                                              | ActivityTrackingOptions.Tags;
        });

        // Configuração do OpenTelemetry Tracing e Metrics
        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(resourceBuilder)
                    .AddSource(openTelemetryOptions.ServiceName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddNpgsql()
                    .AddSqlClientInstrumentation(opt => opt.SetDbStatementForText = true)
                    .AddRedisInstrumentation(opt => opt.SetVerboseDatabaseStatements = true)
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddMongoDBInstrumentation();

                if (openTelemetryOptions.Exporters.Contains(ExporterTypes.OTLP))
                    tracing.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri($"{openTelemetryOptions.Url}/v1/traces");
                        options.Protocol = OtlpExportProtocol.HttpProtobuf;
                    });

                if (openTelemetryOptions.Exporters.Contains(ExporterTypes.Console))
                    tracing.AddConsoleExporter();
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                    .AddMeter("System.Net.Http")
                    .AddMeter("System.Net.NameResolution")
                    .AddMeter(openTelemetryOptions.ServiceName);

                if (openTelemetryOptions.Exporters.Contains(ExporterTypes.OTLP))
                    metrics.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri($"{openTelemetryOptions.Url}/v1/metrics");
                        options.Protocol = OtlpExportProtocol.HttpProtobuf;
                    });

                if (openTelemetryOptions.Exporters.Contains(ExporterTypes.Console))
                    metrics.AddConsoleExporter();
            });
    }
}