using Microsoft.AspNetCore.Identity;
using UserService.Domain.Users;
using UserService.Infrastructure.IdentityEntities;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<UserDomain?> GetUserById(string id)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());
        if (identityUser == null)
        {
            return null;
        }

        return new UserDomain { 
            Id = identityUser.Id,
            Username = identityUser.UserName ?? string.Empty,
            Email = identityUser.Email ?? string.Empty,
            FirstName = identityUser.FirstName,
            LastName = identityUser.LastName,
            Address = identityUser.Address,
            DateOfBirth = identityUser.DateOfBirth,
            IsActive = identityUser.IsActive
        };
    }
    public async Task CreateUser(UserDomain user, string password)
    {
        var identityUser = new User
        {
            Id = user.Id,
            UserName = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };
        await _userManager.CreateAsync(identityUser, password);
    }
}
