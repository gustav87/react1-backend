using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace React1_Backend.Authentication;

[ApiController]
[Route("api/[controller]")]
public class CookieController(ILogger<CookieController> logger, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly ILogger<CookieController> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    [HttpGet]
    public async Task<IActionResult> GetCookie()
    {
        ApplicationUser user = await AuthenticateUser("maria.rodriguez@contoso.com", "asdf");

        List<Claim> claims =
        [
            new(ClaimTypes.Name, user.Email),
            new("FullName", user.FullName),
            new(ClaimTypes.Role, "Administrator"),
            new Claim(ClaimTypes.Email, "maria.rodriguez@contoso.com"),
        ];

        // var claims = new List<Claim>
        // {
        //     new(ClaimTypes.Name, user.Email),
        //     new("FullName", user.FullName),
        //     new(ClaimTypes.Role, "Administrator"),
        // };

        string cookieAuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        // string cookieAuthenticationScheme = "MyCookieScheme";

        ClaimsIdentity claimsIdentity = new(claims, cookieAuthenticationScheme);
        AuthenticationProperties authProperties = new();

        await _httpContextAccessor.HttpContext.SignInAsync(
            cookieAuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );

        _logger.LogInformation("User {Email} logged in at {Time}.", user.Email, DateTime.UtcNow);
        return Ok();
    }

    private static async Task<ApplicationUser> AuthenticateUser(string email, string password)
    {
        // For demonstration purposes, authenticate a user
        // with a static email address. Ignore the password.
        // Assume that checking the database takes 500ms

        await Task.Delay(500);

        if (email == "maria.rodriguez@contoso.com")
        {
            return new ApplicationUser()
            {
                Email = "maria.rodriguez@contoso.com",
                FullName = "Maria Rodriguez",
            };
        }
        else
        {
            return null;
        }
    }
}

public class ApplicationUser
{
    public string Email { get; set; }
    public string FullName { get; set; }
}
