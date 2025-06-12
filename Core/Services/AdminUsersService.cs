using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace BusinessLogic.Services;


public class AdminUsersService : IAdminUsersService
{
    private readonly UserManager<IdentityUser> _userManager;

    public AdminUsersService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<IdentityResult> CreateUserAsync(UserDto user)
    {
        var identityUser = new IdentityUser
        {
            UserName = user.UserName ?? user.Email,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        var passwordHash = _userManager.PasswordHasher.HashPassword(identityUser, user.Password);
        identityUser.PasswordHash = passwordHash;
        return await _userManager.CreateAsync(identityUser); 
    }

    public async Task<IdentityResult> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {id} not found.");
        }
        return await _userManager.DeleteAsync(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _userManager.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                UserName = u.UserName
            }).ToListAsync();
    }

    public async Task<UserDto?> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id)
            ?? throw new ArgumentException($"User with ID {id} not found.");
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<IdentityResult> UpdateUserAsync(UserDto user)
    {
        var identityUser = await _userManager.FindByIdAsync(user.Id);
        if (identityUser == null)
        {
            throw new ArgumentException($"User with ID {user.Id} not found.");
        }
        identityUser.Email = user.Email;
        identityUser.PhoneNumber = user.PhoneNumber;
        identityUser.UserName = user.UserName;
        if (!string.IsNullOrEmpty(user.Password))
        {
            identityUser.PasswordHash = _userManager.PasswordHasher.HashPassword(identityUser, user.Password);
        }
        return await _userManager.UpdateAsync(identityUser);
    }
}
