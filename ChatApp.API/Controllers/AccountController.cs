using ChatApp.API.Data;
using ChatApp.API.DTOs;
using ChatApp.API.Entities;
using ChatApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(ApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisteDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
        using var hmac = new HMACSHA512();
        var user = new User
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var userDto = new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        return Ok(userDto);
    }

    [HttpPost("acc")]
    public async Task<IActionResult> Acc(AddUserDto registeDto)
    {
        using var hmac = new HMACSHA512();
        var user = new User
        {
            UserName = registeDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registeDto.Password)),
            PasswordSalt = hmac.Key,
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var userDto = new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
        return Ok(userDto);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.UserName.ToLower());
            if (user == null) return Unauthorized("Invalid creadintials");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid creadintials");
            }
            var userDto = new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
            return Ok(userDto);
        }
        catch
        {
            return Unauthorized("Invalid creadintials");
        }
    }
    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
    }
}
