namespace ChatApp.API.DTOs;

public class UpdateUserDto
{
    public string? Bio { get; set; }

    public string? Interests { get; set; }

    public string? City { get; set; }
    public string? Country { get; set; }
    public DateTime DateOfBirth { get; set; }
}
