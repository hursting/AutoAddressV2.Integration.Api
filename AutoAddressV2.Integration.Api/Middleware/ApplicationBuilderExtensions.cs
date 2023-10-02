using AutoAddressV2.Integration.Api.Http.PipeLine;
using AutoAddressV2.Integration.Api.Swagger;

namespace AutoAddressV2.Integration.Api.Middleware;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSwaggerDocumentation( app.Environment);
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        //app.UseSerilogRequestLogging();
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        //app.UseMiddleware<HttpHeaderValidationMiddleware>();
        
        return app;
    }
    
}