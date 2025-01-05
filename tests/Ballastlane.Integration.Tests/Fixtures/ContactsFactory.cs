using Microsoft.AspNetCore.Mvc.Testing;

namespace Ballastlane.Integration.Tests.Fixtures;

public class ContactsFactory : WebApplicationFactory<Contacts.Api.Program>
{
    public string ContactIdCreated { get; set; } = string.Empty;
}
