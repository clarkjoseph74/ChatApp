using AutoMapper;
using ChatApp.API.DTOs;
using ChatApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
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
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _repo.GetByNameAsync(username!);
        _mapper.Map(dto, user);
        _repo.Update(user);
        if (await _repo.SaveAllAsync()) return NoContent();
        return BadRequest("Error While Updating the user");
    }
}
