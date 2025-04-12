using System.Security.Claims;

public class LogRequest
{
    private readonly RequestDelegate _next;

    public LogRequest(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<LogRequest> logger)
    {
        logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

        // Call the next middleware in the pipeline
        await _next(context);
    }
}
