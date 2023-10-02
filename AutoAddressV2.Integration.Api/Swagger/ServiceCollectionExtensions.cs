using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace AutoAddressV2.Integration.Api.Swagger;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerGeneratorOptions.IgnoreObsoleteActions = true;
            options.CustomSchemaIds( type => type.ToString());
            options.AddEnumsWithValuesFixFilters();
        });
        
        
        services.AddSwaggerGenNewtonsoftSupport();
        
        return services;
    }
    
    internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Applied.Integration.Api");
        });
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        return app;
    }
    
}