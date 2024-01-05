using Microsoft.AspNetCore.Mvc.Filters;

namespace react1_backend.Filters.ActionFilters;

public class AsyncActionFilterExample : Attribute, IAsyncActionFilter
{
    public string? PermissionName { get; set; }
    private readonly string adminToken = Environment.GetEnvironmentVariable("adminToken") ?? "";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Do something before the action executes.
        var containsToken = context.HttpContext.Request.Query.ContainsKey("token");
        Console.WriteLine(PermissionName);

        if (!containsToken)
        {
            context.HttpContext.Response.StatusCode = 401; // BadRequest
            await context.HttpContext.Response.WriteAsync("Token missing.");
            return;
        }

        var tokenValue = context.HttpContext.Request.Query["token"][0];

        if (!ValidateToken(tokenValue))
        {
            context.HttpContext.Response.StatusCode = 401; // BadRequest
            await context.HttpContext.Response.WriteAsync("Token validation failed.");
            return;
        }

        var resultContext = await next();
    }

    private bool ValidateToken(string token) {
        return token == adminToken;
    }
}
