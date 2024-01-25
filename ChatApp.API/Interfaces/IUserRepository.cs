using ChatApp.API.DTOs;
using ChatApp.API.Entities;

namespace ChatApp.API.Interfaces;

public interface IUserRepository
{
    void Update(User user);

    Task<bool> SaveAllAsync();
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(int id);
    Task<User> GetByNameAsync(string name);
    Task<IEnumerable<MemberDto>> GetAllMembersAsync();
    Task<MemberDto?> GetMemberByNameAsync(string name);
}
