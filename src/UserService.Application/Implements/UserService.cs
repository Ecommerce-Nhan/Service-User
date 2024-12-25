using UserService.Application.Interfaces;
using SharedLibrary.Dtos.Users;
using UserService.Entities;
using UserService.Entities.Abstractions;

namespace UserService.Application.Implements;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> CreateUserAsync(CreateUserDto input)
    {
        var model = new User
        {
            UserName = input.UserName,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            FirstName = input.FirstName,
            LastName = input.LastName,
            Address = input.Address,
            DateOfBirth = input.DateOfBirth,
            IsActive = false
        };

        return await _userRepository.CreateUserAsync(model);
    }

    public async Task<UserDto?> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return new UserDto(user.Id, user.FirstName, user.LastName, user.UserName, user.Address, user.Email, user.PhoneNumber, user.DateOfBirth);
    }
}
