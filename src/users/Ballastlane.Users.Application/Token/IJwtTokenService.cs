using Ballastlane.Users.Domain.Entities;

namespace Ballastlane.Users.Application.Token
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}