using System.Data;

namespace Ballastlane.Users.Infrastructure.Repositories;

public interface ISqlServer
{
    int ExecuteNonQuery(string query, object? parameters = null, CancellationToken cancellationToken = default);
    IDataReader ExecuteReader(string query, object? parameters = null, CancellationToken cancellationToken = default);
    object? ExecuteScalar(string query, object? parameters = null, CancellationToken cancellationToken = default);
}
