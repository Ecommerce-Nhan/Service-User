using Microsoft.AspNetCore.Identity;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace UserService.Entities.Abstractions;

public interface IUserRepository
{
    Task<PagedResponse<List<User>>> GetAllAsync(PaginationFilter pageFilter);
    Task<User?> GetUserByIdAsync(string id);
    Task<IdentityResult> CreateUserAsync(User user, string password);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<IdentityResult> DeleteUserAsync(User user);
}