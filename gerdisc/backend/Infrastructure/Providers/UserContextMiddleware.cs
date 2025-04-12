using System.Security.Claims;
using saga.Models.Enums;

namespace saga.Infrastructure.Providers
{
    /// <summary>
    /// Middleware for setting user context based on the claims of the current HttpContext.
    /// </summary>
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContextMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next request delegate in the pipeline.</param>
        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Sets the user context based on the claims of the current HttpContext.
        /// </summary>
        /// <param name="context">The current HttpContext.</param>
        /// <param name="userContext">The user context to be populated.</param>
        public async Task InvokeAsync(HttpContext context, IUserContext userContext)
        {
            string? userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? role = context.User?.FindFirst(ClaimTypes.Role)?.Value;
            var user = Guid.TryParse(userId, out Guid id);

            if (Enum.TryParse<RolesEnum>(role, out var enumValue))
            {
                // Set the user information in the UserContext
                userContext.UserId = id;
                userContext.Role = enumValue;
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
