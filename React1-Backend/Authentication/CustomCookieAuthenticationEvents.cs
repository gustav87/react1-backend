using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace React1_Backend.Authentication;

public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
{
    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        ClaimsPrincipal userPrincipal = context.Principal;

        // Look for the LastChanged claim.
        string lastChanged = (from c in userPrincipal.Claims
                              where c.Type == "LastChanged"
                              select c.Value).FirstOrDefault();

        // if (string.IsNullOrEmpty(lastChanged))
        // {
        //     context.RejectPrincipal();
        //     await context.HttpContext.SignOutAsync(
        //         CookieAuthenticationDefaults.AuthenticationScheme);
        // }
    }
}
