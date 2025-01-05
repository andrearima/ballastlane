using Ballastlane.Users.Application.Models;

namespace Ballastlane.Users.Application.Apps;

public interface IUserApp
{
    Task<UserResponse?> CreateUser(UpsertUser createUser, CancellationToken cancellationToken);
    Task<UserResponse?> GetUser(int userId, CancellationToken cancellationToken);
    Task<IEnumerable<UserResponse>> GetUsers(CancellationToken cancellationToken);
    Task<bool> DeleteUser(int userId, CancellationToken cancellationToken);
    Task<UserResponse?> UpdateUser(int userId, UpsertUser updateUser, CancellationToken cancellationToken);
}
