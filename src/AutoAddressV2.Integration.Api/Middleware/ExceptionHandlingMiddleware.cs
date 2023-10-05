using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace AutoAddressV2.Integration.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;
    private const string JsonContentType = "application/json";
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment environment, ILogger<ExceptionHandlingMiddleware>  logger)
    {
        _next = next;

        _environment = environment;

        _logger = logger;

    }
    
    
    
    public async  Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (HttpRequestException httpRequestException)
        {
            _logger.LogError(httpRequestException, "Request Exception");
            await HandleExceptionAsync(httpContext,httpRequestException);
        }
        catch (TaskCanceledException taskCancelledException)
        {
            _logger.LogError(taskCancelledException, "Task Cancelled Exception");
            await HandleExceptionAsync(httpContext,taskCancelledException);
        }
        catch (TimeoutException timeoutException)
        {
            _logger.LogError(timeoutException, "Timeout Exception");
            await HandleExceptionAsync(httpContext,timeoutException);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unknown Exception");
            await HandleExceptionAsync(httpContext,e);
        }
    }
    
    
    private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
    {
        httpContext.Response.ContentType = JsonContentType;
        
        httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        string message = "Internal Server Error";
        
        if (e.GetType()==typeof(TimeoutException) || e.GetType() == typeof(TaskCanceledException))
        {
            message = e.Message;
            httpContext.Response.StatusCode = (int) HttpStatusCode.RequestTimeout;
        }
        
        var content = JsonConvert.SerializeObject(new JsonResult()
            {Success = false,StatusCode = httpContext.Response.StatusCode, Error = e.Message});
        
        return httpContext.Response.WriteAsync(content);
        
    }


}