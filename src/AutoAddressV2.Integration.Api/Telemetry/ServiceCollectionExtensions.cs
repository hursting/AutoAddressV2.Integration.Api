using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AutoAddressV2.Integration.Api.Telemetry;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutoAddressTelemetry(this IServiceCollection services)
    {
        
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService("SomeServiceNaem");

        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.SetResourceBuilder(resourceBuilder)
                    .AddPrometheusExporter()
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    //.AddHttpClientInstrumentation()
                    .AddEventCountersInstrumentation(c =>
                    {
                        // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/available-counters
                        c.AddEventSources(
                            "Microsoft.AspNetCore.Hosting",
                            "Microsoft-AspNetCore-Server-Kestrel",
                            "System.Net.Http",
                            "System.Net.Sockets",
                            "System.Net.NameResolution",
                            "System.Net.Security");
                    });
            })
            .WithTracing(tracing =>
            {
                // We need to use AlwaysSampler to record spans
                // from Todo.Web.Server, because there it no OpenTelemetry
                // instrumentation
                tracing.SetResourceBuilder(resourceBuilder)
                    .SetSampler(new AlwaysOnSampler())
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation();

                // if (!string.IsNullOrWhiteSpace(otlpEndpoint))
                // {
                //     tracing.AddOtlpExporter();
                // }
            })
            .StartWithHost();
        
        return services;
    }
}