using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Ballastlane.Notification;

namespace Ballastlane.Filters;

public sealed class ResultFilter : IAsyncAlwaysRunResultFilter
{
    private readonly INotifications _notifications;

    public ResultFilter(INotifications notifications)
    {
        _notifications = notifications;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (!_notifications.IsValid)
        {
            context.HttpContext.Response.StatusCode = 400;
            context.Result = new JsonResult(_notifications.GetNotifications());
        }

        await next.Invoke();
    }
}
