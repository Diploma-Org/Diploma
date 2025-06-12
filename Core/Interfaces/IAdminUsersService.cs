using Microsoft.AspNetCore.Identity;
using BusinessLogic.DTOs;
namespace BusinessLogic.Interfaces;

public interface IAdminUsersService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(string id);
    Task<IdentityResult> CreateUserAsync(UserDto user);
    Task<IdentityResult> UpdateUserAsync(UserDto user);
    Task<IdentityResult> DeleteUserAsync(string id);
}
