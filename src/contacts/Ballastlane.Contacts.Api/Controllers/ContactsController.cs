using Ballastlane.Contacts.Application.Apps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.Contacts.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactApp _contactsApp;

    public ContactsController(IContactApp contactsApp)
    {
        _contactsApp = contactsApp;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var ownerId))
        {
            var contacts = await _contactsApp.GetContacts(ownerId, cancellationToken);
            if (contacts.Any())
                return Ok(contacts);
        }

        return NotFound();
    }

    private bool TryGetUserId(out int userId)
    {
        var userid = ControllerContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
        if (int.TryParse(userid, out userId))
            return true;

        return false;
    }
}
