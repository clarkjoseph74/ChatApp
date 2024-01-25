namespace ChatApp.API.DTOs;

public class AddUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }

    public string Password { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string KnownAs { get; set; }
    public string Gender { get; set; }

    public string Bio { get; set; }

    public string Interests { get; set; }

    public string City { get; set; }
    public string Country { get; set; }


}
