using Ballastlane.Integration.Tests.Fixtures;

namespace Ballastlane.Integration.Tests;

public class AuthenticationController : IClassFixture<AuthenticationFactory>
{
    private readonly AuthenticationFactory _authenticationFactory;
    public AuthenticationController(AuthenticationFactory authenticationFactory)
    {
        _authenticationFactory = authenticationFactory;
    }

    [Fact]
    public async Task Post_Authentication_MustReturn_JwtToken()
    {
        // Arrange
        var client = _authenticationFactory.CreateDefaultClient();
        var httpContent = _authenticationFactory.CreateLoginRequest("admin@admin.com", "admin");

        //Act
        var result = await client.PostAsync("/authentication", httpContent);
        var jwtToken = await result.Content.ReadAsStringAsync();

        // Assert
        Assert.NotNull(jwtToken);
        Assert.Contains(".", jwtToken);
    }

    [Fact]
    public async Task Post_Authentication_MustReturn_StatusCode_401_WhenInvalidEmail()
    {
        // Arrange
        var client = _authenticationFactory.CreateDefaultClient();
        var httpContent = _authenticationFactory.CreateLoginRequest("nonExistent@admin.com", "admin");

        // Act
        var result = await client.PostAsync("/authentication", httpContent);
        var response = await result.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, result.StatusCode);
        Assert.NotNull(response);
        Assert.Equal("Email or Password is Invalid.", response);
    }

    [Fact]
    public async Task Post_Authentication_MustReturn_StatusCode_401_WhenInvalidPassword()
    {
        // Arrange
        var client = _authenticationFactory.CreateDefaultClient();
        var httpContent = _authenticationFactory.CreateLoginRequest("admin@admin.com", "worngPassword");

        // Act
        var result = await client.PostAsync("/authentication", httpContent);
        var response = await result.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, result.StatusCode);
        Assert.NotNull(response);
        Assert.Equal("Email or Password is Invalid.", response);
    }
}