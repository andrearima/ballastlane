using Ballastlane.Users.Domain.Entities;

namespace Ballastlane.Users.Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken);
    Task<User?> GetUser(int userId, CancellationToken cancellationToken);
    Task<User?> GetUser(string email, CancellationToken cancellationToken);
    Task<User?> CreateUser(User user, CancellationToken cancellationToken);
    Task<User?> UpdateUser(User user, CancellationToken cancellationToken);
    Task<bool> DeleteUser(User user, CancellationToken cancellationToken);
}
