using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;
using UserService.Entities;
using UserService.Entities.Abstractions;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(UserManager<User> userManager) : IUserRepository
{
    public async Task<PagedResponse<List<User>>> GetAllAsync(PaginationFilter pageFilter)
    {
        var validFilter = new PaginationFilter(pageFilter.PageNumber, pageFilter.PageSize);
        var query = userManager.Users.AsNoTracking();

        var pagedData = await query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                   .Take(validFilter.PageSize)
                                   .ToListAsync();
        var totalRecords = await query.CountAsync();
        var response = new PagedResponse<List<User>>(pagedData, validFilter.PageNumber, validFilter.PageSize);

        var totalPages = ((double)totalRecords / validFilter.PageSize);
        response.TotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        response.TotalRecords = totalRecords;
        return response;
    }
    public async Task<User?> GetUserByIdAsync(string id) =>
        await userManager.FindByIdAsync(id.ToString());
    public async Task<IdentityResult> CreateUserAsync(User user, string password) =>
        await userManager.CreateAsync(user, password);
    public async Task<IdentityResult> UpdateUserAsync(User user) =>
        await userManager.UpdateAsync(user);
    public async Task<IdentityResult> DeleteUserAsync(User user) =>
        await userManager.DeleteAsync(user);
}