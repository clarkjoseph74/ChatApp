namespace ChatApp.API.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int Age { get; set; }
    public string ImageUrl { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public string? KnownAs { get; set; }

    public DateTime LastActive { get; set; } = DateTime.Now;

    public string? Gender { get; set; }

    public string? Bio { get; set; }

    public string? Interests { get; set; }

    public string? City { get; set; }
    public string? Country { get; set; }

    public ICollection<PhotoDto>? Photos { get; set; }
}
