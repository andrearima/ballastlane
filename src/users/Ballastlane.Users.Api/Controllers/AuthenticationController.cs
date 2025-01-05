using Ballastlane.Notification;
using Ballastlane.Users.Application.Apps;
using Ballastlane.Users.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.Users.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AuthenticationController : Controller
{
    private readonly INotifications _notifications;
    private readonly ILoginApp _loginApp;
    public AuthenticationController(ILoginApp loginApp, INotifications notifications)
    {
        _loginApp = loginApp;
        _notifications = notifications;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var token = await _loginApp.Login(request, cancellationToken);
        if (_notifications.IsValid && token is not null)
        {
            return Ok(token);
        }

        if (!_notifications.IsValid)
        {
            var message = _notifications.GetNotifications().FirstOrDefault();
            _notifications.Clear();
            return Unauthorized(message);
        }

        return Unauthorized("Email or password invalid.");
    }
}
