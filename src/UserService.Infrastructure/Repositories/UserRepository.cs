using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Entities;
using UserService.Entities.Abstractions;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync() =>
        await userManager.Users.AsNoTracking().ToListAsync();
    public async Task<User?> GetUserByIdAsync(string id) =>
        await userManager.FindByIdAsync(id.ToString());
    public async Task<IdentityResult> CreateUserAsync(User user) =>
        await userManager.CreateAsync(user);
    public async Task<IdentityResult> UpdateUserAsync(User user) =>
        await userManager.UpdateAsync(user);
    public async Task<IdentityResult> DeleteUserAsync(User user) =>
        await userManager.DeleteAsync(user);
}
