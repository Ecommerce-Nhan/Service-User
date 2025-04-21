using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos.Users;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;
using UserService.Application.Interfaces;
using UserService.Entities;
using UserService.Entities.Abstractions;

namespace UserService.Application.Implements;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<PagedResponse<List<UserDto>>> GetAll(PaginationFilter pagination)
    {
        var pagedData = await userRepository.GetAllAsync(pagination);
        var result = mapper.Map<PagedResponse<List<UserDto>>>(pagedData);

        return result;
    }
    public async Task<UserDto?> GetUserByIdAsync(string id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
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

        var identityResult = await userRepository.CreateUserAsync(model, input.Password);
        return identityResult.Succeeded;
    }
    public async Task<bool> UpdateUserAsync(string id, UpdateUserDto input)
    {
        var user = await userRepository.GetUserByIdAsync(id);
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

        var identityResult = await userRepository.UpdateUserAsync(user);
        return identityResult.Succeeded;
    }
    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        var identityResult = await userRepository.DeleteUserAsync(user);
        return identityResult.Succeeded;
    }
}