using System.Text.Json;
using AutoAddressV2.Integration.Api.Authentication;
using AutoAddressV2.Integration.Api.Caching;
using AutoAddressV2.Integration.Api.Configuration;
using AutoAddressV2.Integration.Api.Http;
using AutoAddressV2.Integration.Api.Swagger;
using AutoAddressV2.Integration.Api.Telemetry;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace AutoAddressV2.Integration.Api.Middleware;

public static class RegisterStartupServices
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerDocumentation();
        builder.Configuration.AddEnvironmentVariables();
        
        AppSettings appSettings = new AppSettings();
        
        builder.Configuration.Bind(appSettings);

        builder.Services.Configure<AppSettings>(builder.Configuration);
        
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAutoAddressCaching();

        builder.Services.AddAutoAddressTelemetry();
        
        builder.Services.AddAutoAddressHttpClient(builder.Configuration,null);
        
        builder.Services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
        });


        builder.Services.AddTransient<IProvideAuthentication, AuthenticationService>();
        
        
        return builder;
    }
}