using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos.Users;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;
using UserService.Application.Interfaces;
using UserService.Domains.Entities;

namespace UserService.Application.Implements;

public class UserService(UserManager<User> userManager, IMapper mapper) : IUserService
{
    public async Task<PagedResponse<List<UserDto>>> GetAll(PaginationFilter pagination)
    {
        var pagedData = await GetPagedDataAsync(pagination);
        var result = mapper.Map<PagedResponse<List<UserDto>>>(pagedData);

        return result;
    }
    public async Task<UserDto?> GetUserByIdAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        var result = mapper.Map<UserDto>(user);

        return result;
    }
    public async Task<bool> CreateUserAsync(CreateUserDto input)
    {
        var hasher = new PasswordHasher<User>();
        var model = new User
        {
            UserName = input.UserName,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            FirstName = input.FirstName,
            LastName = input.LastName,
            Address = input.Address,
            DateOfBirth = input.DateOfBirth,
            IsActive = false,
            PhoneNumberConfirmed = false,
            EmailConfirmed = false
        };

        var identityResult = await userManager.CreateAsync(model, input.Password);
        return identityResult.Succeeded;
    }
    public async Task<bool> UpdateUserAsync(string id, UpdateUserDto input)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return false;
        }
        user.UserName = input.UserName;
        user.Email = input.Email;
        user.PhoneNumber = input.PhoneNumber;
        user.FirstName = input.FirstName;
        user.LastName = input.LastName;
        user.Address = input.Address;
        user.DateOfBirth = input.DateOfBirth;

        var identityResult = await userManager.UpdateAsync(user);
        return identityResult.Succeeded;
    }
    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        var identityResult = await userManager.DeleteAsync(user);
        return identityResult.Succeeded;
    }

    private async Task<PagedResponse<List<User>>> GetPagedDataAsync(PaginationFilter pageFilter)
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
}