using Ballastlane.Users.Application.Apps;
using Ballastlane.Users.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ballastlane.Users.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserApp _userApp;
    public UserController(ILogger<UserController> logger, IUserApp userApp)
    {
        _logger = logger;
        _userApp = userApp;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> Get([FromRoute] int userId, CancellationToken cancellationToken)
    {
        var user = await _userApp.GetUser(userId, cancellationToken);
        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertUser user, CancellationToken cancellationToken)
    {
        var userResponse = await _userApp.CreateUser(user, cancellationToken);
        if (userResponse is null)
            return BadRequest();

        return Ok(userResponse);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> Update([FromRoute] int userId, [FromBody] UpsertUser user, CancellationToken cancellationToken)
    {
        var userResponse = await _userApp.UpdateUser(userId, user, cancellationToken);
        if (userResponse is null)
            return BadRequest();

        return Ok(userResponse);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete([FromRoute] int userId, CancellationToken cancellationToken)
    {
        var deleted = await _userApp.DeleteUser(userId, cancellationToken);
        if (deleted)
            return NoContent();

        return NotFound();
    }
}