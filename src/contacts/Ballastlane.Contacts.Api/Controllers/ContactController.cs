using Ballastlane.Contacts.Application.Apps;
using Ballastlane.Contacts.Application.Models;
using Ballastlane.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.Contacts.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactApp _contactsApp;
    private readonly INotifications _notifications;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IContactApp contactsApp, ILogger<ContactController> logger, INotifications notifications)
    {
        _contactsApp = contactsApp;
        _logger = logger;
        _notifications = notifications;
    }

    [HttpGet("{contactId}")]
    public async Task<IActionResult> Get([FromRoute] string contactId, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var ownerId))
        {
            var contact = await _contactsApp.GetContact(ownerId, contactId, cancellationToken);
            if (contact is null)
                return NotFound();

            return Ok(contact);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContact createContact, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var ownerId))
        {
            var result = await _contactsApp.CreateContact(ownerId, createContact, cancellationToken);
            return Ok(result);
        }

        return BadRequest();
    }

    [HttpPut("{contactId}")]
    public async Task<IActionResult> Update([FromRoute] string contactId, [FromBody] UpdateContact updateContact, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var ownerId))
        {
            var result = await _contactsApp.UpdateContact(ownerId, contactId, updateContact, cancellationToken);
            if (_notifications.IsValid && result is null)
                return NotFound();

            return Ok(result);
        }

        return BadRequest();
    }

    [HttpDelete("{contactId}")]
    public async Task<IActionResult> Delete([FromRoute] string contactId, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var ownerId))
        {
            var result = await _contactsApp.DeleteContact(ownerId, contactId, cancellationToken);
            if (_notifications.IsValid && !result)
                return NotFound();

            return NoContent();
        }

        return BadRequest();
    }

    private bool TryGetUserId(out int userId)
    {
        var userid = ControllerContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
        if (int.TryParse(userid, out userId))
            return true;

        return false;
    }
}
