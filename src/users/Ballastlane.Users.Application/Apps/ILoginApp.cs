using Ballastlane.Users.Application.Models;

namespace Ballastlane.Users.Application.Apps;

public interface ILoginApp
{
    Task<string?> Login(LoginRequest loginRequest, CancellationToken cancellationToken);
}