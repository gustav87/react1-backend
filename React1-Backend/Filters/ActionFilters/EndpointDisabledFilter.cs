using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace React1_Backend.Filters.ActionFilters;

public class EndpointDisabledFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.HttpContext.Response.StatusCode = 501; // 501 Not Implemented
        await context.HttpContext.Response.WriteAsync("Endpoint disabled.");
    }
}
