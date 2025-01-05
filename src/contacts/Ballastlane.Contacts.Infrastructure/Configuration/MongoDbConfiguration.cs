using MongoDB.Driver;

namespace Ballastlane.Contacts.Infrastructure.Configuration;

public sealed class MongoDbConfiguration
{
    public string? ConnectionString { get; set; }
    public string? Database { get; set; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString))
            throw new MongoConfigurationException("Connection string is required.");

        if (string.IsNullOrWhiteSpace(Database))
            throw new MongoConfigurationException("Database is required.");
    }
}
