using Microsoft.AspNetCore.Identity;
using UserService.Entities;
using UserService.Entities.Abstractions;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<User?> GetUserByIdAsync(string id)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());

        return identityUser;
    }
    public async Task<bool> CreateUserAsync(User user)
    {
        var identityResult = await _userManager.CreateAsync(user);

        return identityResult.Succeeded;
    }
}
