using Ballastlane.Contacts.Application.Models;
using Ballastlane.Contacts.Domain;
using Ballastlane.Integration.Tests.Fixtures;
using Ballastlane.Integration.Tests.Order;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Ballastlane.Integration.Tests;

[TestCaseOrderer("Ballastlane.Integration.Tests.Order.PriorityOrderer", "Ballastlane.Integration.Tests")]
public class ContactControllerTests : IClassFixture<ContactsFactory>, IClassFixture<AuthenticationFactory>
{
    private readonly ContactsFactory _contactsfactory;
    private readonly AuthenticationFactory _authenticationFactory;
    public ContactControllerTests(ContactsFactory contactsfactory, AuthenticationFactory authenticationFactory)
    {
        _contactsfactory = contactsfactory;
        _authenticationFactory = authenticationFactory;
    }

    [Fact, Priority(0)]
    public async Task Get_MustReturn_401()
    {
        // Assert
        var client = _contactsfactory.CreateClient();

        //Act
        var result = await client.GetAsync("/contact/any");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact, Priority(0)]
    public async Task Get_MustReturn_405()
    {
        // Assert
        var client = await CreateClientWithBearerToken();

        //Act
        var result = await client.GetAsync("/contact");

        // Assert
        Assert.Equal(HttpStatusCode.MethodNotAllowed, result.StatusCode);
    }

    [Fact, Priority(0)]
    public async Task Get_MustReturn_404()
    {
        // Assert
        var client = await CreateClientWithBearerToken();

        //Act
        var result = await client.GetAsync("/contact/non-existent");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact, Priority(0)]
    public async Task Post_MustReturn_401()
    {
        // Assert
        var client = _contactsfactory.CreateClient();
        var content = _authenticationFactory.CreateContent(new CreateContact { FirstName = "test" });

        //Act
        var result = await client.PostAsync("/contact", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    /// <summary>
    /// Scenario 2: Create a Contact
    /// </summary>
    [Fact, Priority(1)]
    public async Task Post_MustReturn_CreateContact()
    {
        // Assert
        var client = await CreateClientWithBearerToken();
        var content = _authenticationFactory.CreateContent(new CreateContact { FirstName = "test" });

        //Act
        var result = await client.PostAsync("/contact", content);
        var contact = await result.Content.ReadFromJsonAsync<Contact>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(contact);
        Assert.Equal("test", contact.FirstName);
        _contactsfactory.ContactIdCreated = contact.Id.ToString();
    }

    /// <summary>
    /// Scenario 1: Retrieve a Contact
    /// </summary>
    [Fact, Priority(2)]
    public async Task Get_MustReturn_CreatedContact()
    {
        // Assert
        var client = await CreateClientWithBearerToken();

        //Act
        var result = await client.GetAsync($"/contact/{_contactsfactory.ContactIdCreated}");
        var contact = await result.Content.ReadFromJsonAsync<Contact>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(contact);
        Assert.Equal("test", contact.FirstName);
        Assert.Null(contact.LastName);
    }

    /// <summary>
    /// Scenario 3: Update a Contact
    /// </summary>
    [Fact, Priority(3)]
    public async Task Put_MustReturn_ModifiedContact()
    {
        // Assert
        var client = await CreateClientWithBearerToken();
        var content = _authenticationFactory.CreateContent(new UpdateContact { FirstName = "test", LastName = "lastName" });

        //Act
        var result = await client.PutAsync($"/contact/{_contactsfactory.ContactIdCreated}", content);
        var contact = await result.Content.ReadFromJsonAsync<Contact>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotNull(contact);
        Assert.Equal("test", contact.FirstName);
        Assert.Equal("lastName", contact.LastName);
    }

    /// <summary>
    /// Scenario 4: Delete a Contact
    /// </summary>
    [Fact, Priority(4)]
    public async Task Delete_MustReturn_204()
    {
        // Assert
        var client = await CreateClientWithBearerToken();

        //Act
        var result = await client.DeleteAsync($"/contact/{_contactsfactory.ContactIdCreated}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
    }

    /// <summary>
    /// Scenario 4: Delete a Contact
    /// </summary>
    [Fact, Priority(4)]
    public async Task Delete_MustReturn_404()
    {
        // Assert
        var client = await CreateClientWithBearerToken();

        //Act
        var result = await client.DeleteAsync($"/contact/inexistent");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    private async Task<HttpClient> CreateClientWithBearerToken()
    {
        var authClient = _authenticationFactory.CreateClient();
        var httpContent = _authenticationFactory.CreateLoginRequest("admin@admin.com", "admin");
        var result = await authClient.PostAsync("/authentication", httpContent);
        var jwtToken = await result.Content.ReadAsStringAsync();

        var client = _contactsfactory.CreateDefaultClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        return client;
    }
}
