using System.Security.Claims;
using saga.Models.Enums;
using Hangfire.Dashboard;

namespace saga.Settings;
public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var logger = context.GetHttpContext().RequestServices.GetService(typeof(ILogger<HangfireDashboardAuthorizationFilter>)) as ILogger<HangfireDashboardAuthorizationFilter>;

        if (httpContext.User.Identity.IsAuthenticated)
        {
            string? role = httpContext.User?.FindFirst(ClaimTypes.Role)?.Value;
            if (Enum.TryParse<RolesEnum>(role, ignoreCase: true, out var enumValue) &&
                enumValue == RolesEnum.Administrator)
            {
                logger.LogInformation("Hangfire dashboard authorization successful.");
                return true;
            }
        }

        logger.LogWarning("Hangfire dashboard authorization failed.");
        return true;
    }
}
