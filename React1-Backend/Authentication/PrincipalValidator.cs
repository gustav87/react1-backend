using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace React1_Backend.Authentication;

public static class PrincipalValidator
{
    public static async Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        // if (context == null) throw new System.ArgumentNullException(nameof(context));
        // System.ArgumentNullException.ThrowIfNull(context);

        // var userId = context.Principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        // if (userId == null)
        // {
        //     context.RejectPrincipal();
        //     return;
        // }

        // Get an instance using DI
        // var dbContext = context.HttpContext.RequestServices.GetRequiredService<IdentityDbContext>();
        // var user = await dbContext.Users.FindByIdAsync(userId);
        // if (user == null)
        // {
        //     context.RejectPrincipal();
        //     return;
        // }
    }
}
