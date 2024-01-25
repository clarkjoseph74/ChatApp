using ChatApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;
    public UsersController(IUserRepository repo)
    {
        _repo = repo;
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _repo.GetAllMembersAsync();
        return Ok(users);
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpGet("name/{username}")]
    public async Task<IActionResult> GetUserByname(string username)
    {
        return Ok(await _repo.GetMemberByNameAsync(username));
    }
}
