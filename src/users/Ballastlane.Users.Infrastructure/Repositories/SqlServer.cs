using Ballastlane.Users.Infrastructure.Extensions;
using System.Data;

namespace Ballastlane.Users.Infrastructure.Repositories;

public sealed class SqlServer : ISqlServer
{
    private readonly IDbConnection _connection;

    public SqlServer(IDbConnection connection)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        EnsureTableAndDefaultUser();
    }

    public object? ExecuteScalar(string query, object? parameters = default, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var command = CreateCommand(query, parameters);

        return command.ExecuteScalar();
    }

    public int ExecuteNonQuery(string query, object? parameters = default, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var command = CreateCommand(query, parameters);

        return command.ExecuteNonQuery();
    }

    public IDataReader ExecuteReader(string query, object? parameters = default, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var command = CreateCommand(query, parameters);

        return command.ExecuteReader();
    }

    private IDbCommand CreateCommand(string query, object? parameters = default)
    {
        var command = _connection.CreateCommand();
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        command.AddParameters(parameters);
        OpenConnection();

        return command;
    }

    private void EnsureTableAndDefaultUser()
    {
        const string UsersTable = @"IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Users'))
                                    BEGIN
                                    CREATE TABLE Users (
                                        Id INT IDENTITY(1,1) PRIMARY KEY,
                                        Name NVARCHAR(100) NOT NULL,
                                        Email NVARCHAR(255) NOT NULL UNIQUE,
                                        Password NVARCHAR(255) NOT NULL
                                    );
                                    END";
        ExecuteNonQuery(UsersTable);

        const string AdminUser = @"IF (NOT EXISTS (SELECT * FROM dbo.Users u WHERE u.Email = 'admin@admin.com'))
                                    BEGIN	
	                                    INSERT INTO Users (Name, Email, Password)
	                                    VALUES ('admin', 'admin@admin.com', 'admin')
                                    END";

        ExecuteNonQuery(AdminUser);
    }

    private void OpenConnection()
    {
        if (_connection.State != ConnectionState.Open)
            _connection.Open();
    }
}