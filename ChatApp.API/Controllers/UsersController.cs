using AutoMapper;
using ChatApp.API.DTOs;
using ChatApp.API.Entities;
using ChatApp.API.Extensions;
using ChatApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository repo, IMapper mapper, IPhotoService photoService)
    {
        _repo = repo;
        _mapper = mapper;
        _photoService = photoService;
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

    [HttpGet("name/{username}", Name = "GetUser")]
    public async Task<IActionResult> GetUserByname(string username)
    {
        return Ok(await _repo.GetMemberByNameAsync(username));
    }
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
    {
        var user = await _repo.GetByNameAsync(HttpContext.User.GetUsername()!);
        _mapper.Map(dto, user);
        _repo.Update(user);
        if (await _repo.SaveAllAsync()) return NoContent();
        return BadRequest("Error While Updating the user");
    }
    [HttpPost("add-photo")]
    public async Task<IActionResult> AddPhoto(IFormFile file)
    {
        var user = await _repo.GetByNameAsync(HttpContext.User.GetUsername()!);
        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error);
        Photo photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };
        if (user.Photos?.Count == 0)
        {
            photo.IsMain = true;
        }
        user.Photos?.Add(photo);
        if (await _repo.SaveAllAsync())
        {
            //return Ok(_mapper.Map<PhotoDto>(photo));
            return CreatedAtRoute("GetUser", new { username = User.GetUsername() }, _mapper.Map<PhotoDto>(photo));
        }
        return BadRequest("Error while uploading the photo");
    }

    [HttpPut("main-photo/{photoId}")]
    public async Task<IActionResult> SetMainPhoto(int photoId)
    {
        User user = await _repo.GetByNameAsync(HttpContext.User.GetUsername()!);

        if (user == null) return BadRequest("User not found!");
        var photo = user.Photos.SingleOrDefault(p => p.Id == photoId);
        if (photo == null) return BadRequest("There is unexpected error");
        if (photo.IsMain) return BadRequest("This Photo is already the main");

        var mainPhot = user.Photos!.SingleOrDefault(p => p.IsMain);

        if (mainPhot != null)
        {
            mainPhot.IsMain = false;
            photo.IsMain = true;
            bool isSaved = await _repo.SaveAllAsync();
            if (isSaved) return NoContent();
        }
        return BadRequest("Failed to set the main photo");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<IActionResult> DeletePhoto(int photoId)
    {
        User user = await _repo.GetByNameAsync(HttpContext.User.GetUsername()!);
        Photo? photoToDelete = user.Photos?.SingleOrDefault(p => p.Id == photoId);
        if (photoToDelete == null) return BadRequest("There is no photo to delete");
        if (photoToDelete.IsMain) return BadRequest("Cannot delete the main photo");

        if (photoToDelete.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photoToDelete.PublicId);
            if (result.Error != null) return BadRequest(result.Error);
        }
        user.Photos?.Remove(photoToDelete);
        if (await _repo.SaveAllAsync()) return Ok();
        return BadRequest("Failed To delete the photo");
    }
}
