using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace saga.Infrastructure.Providers
{
    /// <summary>
    /// Logs request and response (body included in DEV), with correlation id.
    /// Put before MVC in the pipeline.
    /// </summary>
    public class LogRequest
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogRequest> _logger;

        public LogRequest(RequestDelegate next, ILogger<LogRequest> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            // propagate correlation id
            context.Request.Headers["x-request-id"] = traceId;
            context.Response.Headers["x-request-id"] = traceId;

#if DEBUG
            // log request body (DEV)
            context.Request.EnableBuffering();
            string reqBody = string.Empty;
            if (context.Request.ContentLength is > 0)
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                reqBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }
            _logger.LogInformation("HTTP {Method} {Path} traceId={TraceId} body={Body}",
                context.Request.Method, context.Request.Path, traceId, reqBody);

            // capture response
            var originalBody = context.Response.Body;
            await using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            try
            {
                await _next(context);
            }
            finally
            {
                memStream.Position = 0;
                var respText = await new StreamReader(memStream).ReadToEndAsync();
                memStream.Position = 0;
                await memStream.CopyToAsync(originalBody);
                context.Response.Body = originalBody;

                _logger.LogInformation("HTTP {Method} {Path} completed {StatusCode} traceId={TraceId} respBody={Response}",
                    context.Request.Method, context.Request.Path, context.Response.StatusCode, traceId, respText);
            }
#else
            _logger.LogInformation("HTTP {Method} {Path} traceId={TraceId}",
                context.Request.Method, context.Request.Path, traceId);
            await _next(context);
#endif
        }
    }
}
