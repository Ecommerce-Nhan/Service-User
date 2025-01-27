using Microsoft.AspNetCore.Identity;

namespace UserService.Entities.Abstractions;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetUserByIdAsync(string id);
    Task<IdentityResult> CreateUserAsync(User user, string password);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<IdentityResult> DeleteUserAsync(User user);
}