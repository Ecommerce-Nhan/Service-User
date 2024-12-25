using SharedLibrary.Dtos.Users;

namespace UserService.Application.Interfaces;

public interface IUserService
{
    Task<bool> CreateUserAsync(CreateUserDto input);
    Task<UserDto?> GetUserByIdAsync(string id);
}