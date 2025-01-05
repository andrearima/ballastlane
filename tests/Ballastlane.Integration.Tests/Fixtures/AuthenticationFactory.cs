using Ballastlane.Users.Application.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;

namespace Ballastlane.Integration.Tests.Fixtures;

public class AuthenticationFactory : WebApplicationFactory<Users.Api.Program>
{
    public async Task<string> GetBearerToken()
    {
        var client = CreateDefaultClient();
        var httpContent = CreateContent(new LoginRequest { Email = "admin@admin.com", Password = "admin" });

        var result = await client.PostAsync("/authentication", httpContent);
        return await result.Content.ReadAsStringAsync();
    }

    public StringContent CreateLoginRequest(string email, string password)
    {
        return CreateContent(new LoginRequest() { Email = email, Password = password });
    }

    public StringContent CreateContent(object @object)
    {
        var jsonContent = JsonConvert.SerializeObject(@object);
        return new StringContent(jsonContent, Encoding.UTF8, "application/json");
    }
}
