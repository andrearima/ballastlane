using Ballastlane.Users.Domain.Entities;
using Ballastlane.Users.Domain.Repositories;

namespace Ballastlane.Users.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ISqlServer _sqlServer;

    public UserRepository(ISqlServer sqlServer)
    {
        _sqlServer = sqlServer;
    }

    public async Task<User?> CreateUser(User user, CancellationToken cancellationToken)
    {
        const string query = @"
            INSERT INTO Users (Name, Email, Password)
            OUTPUT INSERTED.Id
            VALUES (@Name, @Email, @Password)";

        var parameters = new
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        };

        var result = _sqlServer.ExecuteScalar(query, parameters, cancellationToken);
        if (result != null)
        {
            user.Id = (int)result;
            return user;
        }

        return null;
    }

    public async Task<bool> DeleteUser(User user, CancellationToken cancellationToken)
    {
        const string query = "DELETE FROM Users WHERE Id = @Id";

        var parameter = new
        {
            Id = user.Id
        };

        var rowsAffected = _sqlServer.ExecuteNonQuery(query, parameter, cancellationToken);
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
    {
        const string query = "SELECT Id, Name, Email, Password FROM Users";

        using var reader = _sqlServer.ExecuteReader(query, cancellationToken: cancellationToken);
        var users = new List<User>();
        while (reader.Read())
        {
            users.Add(new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3)
            });
        }

        return users;
    }

    public async Task<User?> GetUser(int id, CancellationToken cancellationToken)
    {
        const string query = "SELECT Id, Name, Email, Password FROM Users WHERE Id = @Id";

        using var reader = _sqlServer.ExecuteReader(query, new { Id = id }, cancellationToken);

        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3)
            };
        }

        return null;
    }

    public async Task<User?> GetUser(string email, CancellationToken cancellationToken)
    {
        const string query = "SELECT Id, Name, Email, Password FROM Users WHERE Email = @Email";

        using var reader = _sqlServer.ExecuteReader(query, new { Email = email }, cancellationToken);

        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3)
            };
        }

        return null;
    }

    public async Task<User?> UpdateUser(User user, CancellationToken cancellationToken)
    {
        const string query = @"
            UPDATE Users
            SET Name = @Name, Email = @Email, Password = @Password
            WHERE Id = @Id";

        var parameter = new
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        };

        var rowsAffected = _sqlServer.ExecuteNonQuery(query, parameter, cancellationToken);

        return rowsAffected > 0 ? user : null;
    }
}