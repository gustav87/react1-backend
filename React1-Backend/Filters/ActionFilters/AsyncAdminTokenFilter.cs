using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace React1_Backend.Filters.ActionFilters;

public class AsyncAdminTokenFilter : Attribute, IAsyncActionFilter
{
    public string PermissionName { get; set; }
    private readonly string ADMIN_TOKEN = Environment.GetEnvironmentVariable("ADMIN_TOKEN") ?? "";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Do something before the action executes.
        bool containsToken = context.HttpContext.Request.Query.ContainsKey("token");
        Console.WriteLine(PermissionName);

        if (!containsToken)
        {
            context.HttpContext.Response.StatusCode = 401; // BadRequest
            await context.HttpContext.Response.WriteAsync("Token missing.");
            return;
        }

        string token = context.HttpContext.Request.Query["token"][0];

        if (!ValidateToken(token))
        {
            context.HttpContext.Response.StatusCode = 401; // BadRequest
            await context.HttpContext.Response.WriteAsync("Token validation failed.");
            return;
        }

        var resultContext = await next();
    }

    private bool ValidateToken(string token)
    {
        if (token is null) return false;
        return token == ADMIN_TOKEN;
    }
}
