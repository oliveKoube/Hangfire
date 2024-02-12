using Hangfire.Models;

namespace Hangfire.Filters;

public class PostValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var driver = context.GetArgument<Driver>(0);
        if(string.IsNullOrEmpty(driver.Name) || driver.DriverName == 0)
            return await Task.FromResult(Results.BadRequest("driver is not valid"));

        return await next(context);
    }
}