using Microsoft.AspNetCore.Http;
using RMS.Domain.Interfaces;
using System.Threading.Tasks;

namespace RMS.WebApi.Filters
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
        {
            // 1. Check if User is tied to a specific branch (from JWT claims)
            var userBranchIdClaim = context.User.Claims.FirstOrDefault(c => c.Type.Equals("BranchID", System.StringComparison.OrdinalIgnoreCase))?.Value;
            
            System.Console.WriteLine($"TenantMiddleware: User={context.User.Identity?.Name}, BranchID Claim={userBranchIdClaim}");

            if (!string.IsNullOrEmpty(userBranchIdClaim) && int.TryParse(userBranchIdClaim, out var forcedBranchId))
            {
                // Strict Security: Force this branch ID for this user
                tenantService.BranchID = forcedBranchId;
                System.Console.WriteLine($"TenantMiddleware: Set BranchID={forcedBranchId} from claim");
            }
            else if (context.Request.Headers.TryGetValue("X-Branch-ID", out var headerBranchIdStr))
            {
                // Global Admin: Respect the header selection
                if (int.TryParse(headerBranchIdStr, out var headerBranchId))
                {
                    tenantService.BranchID = headerBranchId;
                    System.Console.WriteLine($"TenantMiddleware: Set BranchID={headerBranchId} from header");
                }
            }
            
            if (!tenantService.BranchID.HasValue)
            {
                System.Console.WriteLine("TenantMiddleware: No BranchID found, acting as Global Admin.");
            }

            await _next(context);
        }
    }
}
