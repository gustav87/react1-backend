using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace React1_Backend.Filters.ActionFilters;

public class ActionFilterExample(IHttpContextAccessor httpContextAccessor) : IActionFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    private readonly string ADMIN_TOKEN = Environment.GetEnvironmentVariable("ADMIN_TOKEN") ?? "";

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Do something before the action executes.
        string queryString = context.HttpContext.Request.QueryString.Value;
        bool containsToken = context.HttpContext.Request.Query.ContainsKey("token");

        // var token = _httpContextAccessor.HttpContext.Request.Headers["token"].ToString();
        // if (string.IsNullOrEmpty(token))
        if (!containsToken)
        {
            context.HttpContext.Response.StatusCode = 401; // BadRequest
            context.HttpContext.Response.WriteAsync("Token missing.");
            return;
        }

        string tokenValue = context.HttpContext.Request.Query["token"][0];

        if (!ValidateToken(tokenValue))
        {
            context.HttpContext.Response.StatusCode = 401; // BadRequest
            context.HttpContext.Response.WriteAsync("Token validation failed.");
            return;
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do something after the action executes.
        Console.WriteLine("ActionFilterExample executed");
    }

    private bool ValidateToken(string token)
    {
        if (token is null) return false;
        return token == ADMIN_TOKEN;
    }
}
