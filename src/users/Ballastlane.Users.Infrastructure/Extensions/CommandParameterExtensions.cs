using System.Data;

namespace Ballastlane.Users.Infrastructure.Extensions;

internal static class CommandParameterExtensions
{
    internal static IDbCommand AddParameters(this IDbCommand command, object? parameters)
    {
        if (parameters is null) return command;

        var properties = parameters.GetType().GetProperties();
        foreach (var property in properties)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = $"@{property.Name}";
            parameter.Value = property.GetValue(parameters) ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        return command;
    }
}
