using Ballastlane.ConfigureOptions;
using Ballastlane.Contacts.Application.Apps;
using Ballastlane.Contacts.Domain.Repository;
using Ballastlane.Contacts.Infrastructure.Configuration;
using Ballastlane.Contacts.Infrastructure.Repository;
using MongoDB.Driver;

namespace Ballastlane.Contacts.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureOptions<Options>();
        builder.Services.AddAuthenticationWithSwagger(builder.Configuration, "Contacts");
        builder.Services.AddNotifications();

        builder.Services.AddScoped<IContactApp, ContactApp>();
        builder.Services.AddScoped<IContactsRepository, ContactsRepository>();

        builder.Services.AddSingleton(p =>
        {
            const string MongoDbSection = "MongoDb";
            var section = builder.Configuration.GetSection(MongoDbSection);
            var mongoConfig = new MongoDbConfiguration();
            section.Bind(mongoConfig);
            mongoConfig.Validate();

            return mongoConfig;
        });

        builder.Services.AddSingleton(provider =>
        {
            var mongoConfig = provider.GetRequiredService<MongoDbConfiguration>();

            return new MongoClient(mongoConfig.ConnectionString);
        });
        builder.Services.AddSingleton(provider =>
        {
            var mongoConfig = provider.GetRequiredService<MongoDbConfiguration>();
            var mongoClient = provider.GetRequiredService<MongoClient>();

            return mongoClient.GetDatabase(mongoConfig.Database);
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

}
