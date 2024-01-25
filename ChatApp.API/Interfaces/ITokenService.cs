using ChatApp.API.Entities;

namespace ChatApp.API.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
