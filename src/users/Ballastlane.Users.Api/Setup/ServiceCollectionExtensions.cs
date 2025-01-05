using Ballastlane.ConfigureOptions;
using Ballastlane.Users.Application.Apps;
using Ballastlane.Users.Application.Token;
using Ballastlane.Users.Domain.Repositories;
using Ballastlane.Users.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ballastlane.Users.Api.Setup;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddUsersServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<Options>();
        services.AddAuthenticationWithSwagger(configuration, "Authentication");
        services.AddNotifications();

        services.AddDependencies(configuration);

        services.AddScoped<ISqlServer, SqlServer>();
        services.AddScoped<IDbConnection>(provider =>
        {
            return new SqlConnection(configuration.GetConnectionString("SqlServer"));
        });

        return services;
    }

    internal static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ILoginApp, LoginApp>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
