using Microsoft.AspNetCore.Identity;

namespace UserService.Entities.Abstractions;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync();
    Task<IdentityResult> CreateRoleAsync(Role role);
}