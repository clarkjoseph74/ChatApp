using ChatApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BuggyController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public BuggyController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Authorize]
    [HttpGet("auth")]
    public IActionResult Auth()
    {
        return Ok("sercret test");
    }
    [HttpGet("not-found")]
    public IActionResult GetNotFound()
    {
        var thing = _context.Users.Find(-1);

        if (thing == null) return NotFound();
        return Ok(thing);
    }
    [HttpGet("server-error")]
    public IActionResult? GetServerError()
    {
        var thing = _context.Users.Find(-1);
        var thingToReturn = thing.ToString();
        return Ok(thingToReturn);

    }
    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("Bad Request");
    }

}
