using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Entities;
using UserService.Entities.Abstractions;

namespace UserService.Infrastructure.Repositories;

public class RoleRepository(RoleManager<Role> roleManager) : IRoleRepository
{
    public async Task<IEnumerable<Role>> GetAllAsync() =>
       await roleManager.Roles.AsNoTracking().ToListAsync();
    public async Task<IdentityResult> CreateRoleAsync(Role role) =>
        await roleManager.CreateAsync(role);
}