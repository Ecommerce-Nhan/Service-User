using Microsoft.AspNetCore.Identity;
using UserService.Domain.Users;
using UserService.Entities.Users;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<User?> GetUserById(string id)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());

        return identityUser;
    }
    public async Task<bool> CreateUser(User user, string password)
    {
        var identityUser = new User
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };
        var identityResult = await _userManager.CreateAsync(identityUser, password);

        return identityResult.Succeeded;
    }
}
