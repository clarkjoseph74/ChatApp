using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.DTOs;

public class RegisteDto
{
    [Required]
    public string Username { get; set; }
    [Required, MinLength(6)]
    public string Password { get; set; }
}
