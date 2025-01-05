using Ballastlane.Notification;
using Ballastlane.Users.Application.Models;
using Ballastlane.Users.Application.Token;
using Ballastlane.Users.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Ballastlane.Users.Application.Apps;

public class LoginApp : ILoginApp
{
    private readonly INotifications _notifications;
    private readonly IJwtTokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<LoginApp> _logger;

    public LoginApp(IJwtTokenService tokenService, ILogger<LoginApp> logger, IUserRepository userRepository, INotifications notifications)
    {
        _tokenService = tokenService;
        _logger = logger;
        _userRepository = userRepository;
        _notifications = notifications;
    }

    public async Task<string?> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        const string DefaultMessageError = "Email or Password is Invalid.";
        var user = await _userRepository.GetUser(loginRequest.Email, cancellationToken);
        if (user is null)
        {
            _notifications.AddNotification(DefaultMessageError);
            _logger.LogDebug("User not found.");
            return null;
        }

        if (!user.Password.Equals(loginRequest.Password))
        {
            _notifications.AddNotification(DefaultMessageError);
            _logger.LogDebug("Password does not match.");
            return null;
        }

        return _tokenService.GenerateToken(user);
    }

}
