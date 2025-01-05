using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Ballastlane.Filters;

public sealed class ExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.ExceptionHandled) return Task.CompletedTask;
        _logger.LogError("UnknowError: {exception}", context.Exception);

        context.HttpContext.Response.StatusCode = 500;

        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }
}
