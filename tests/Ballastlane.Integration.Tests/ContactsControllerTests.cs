using Ballastlane.Integration.Tests.Fixtures;
using System.Net;

namespace Ballastlane.Integration.Tests;

public class ContactsControllerTests : IClassFixture<ContactsFactory>
{
    private readonly ContactsFactory _contactsfactory;
    public ContactsControllerTests(ContactsFactory contactsfactory)
    {
        _contactsfactory = contactsfactory;
    }

    [Fact]
    public async Task Get_MustReturn_401()
    {
        // Assert
        var client = _contactsfactory.CreateDefaultClient();

        //Act
        var result = await client.GetAsync("/contacts");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized,result.StatusCode);
    }
}