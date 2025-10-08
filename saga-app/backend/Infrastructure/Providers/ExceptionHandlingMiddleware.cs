using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace saga.Infrastructure.Providers
{
    /// <summary>
    /// Catches unhandled exceptions and returns RFC 7807 ProblemDetails.
    /// Also injects a correlation id (x-request-id / traceId).
    /// </summary>
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _log;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext ctx)
        {
            var traceId = Activity.Current?.Id ?? ctx.TraceIdentifier;

            // propagate correlation id
            ctx.Request.Headers["x-request-id"] = traceId;
            ctx.Response.Headers["x-request-id"] = traceId;

            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unhandled exception. traceId={TraceId} path={Path}", traceId, ctx.Request.Path);

                ctx.Response.ContentType = "application/problem+json";
                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

#if DEBUG
                var problem = new
                {
                    type = "https://httpstatuses.io/500",
                    title = "An unexpected error occurred.",
                    status = 500,
                    traceId,
                    detail = ex.Message,
                    stack = ex.StackTrace
                };
#else
                var problem = new
                {
                    type = "https://httpstatuses.io/500",
                    title = "An unexpected error occurred.",
                    status = 500,
                    traceId
                };
#endif
                await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }
    }
}
