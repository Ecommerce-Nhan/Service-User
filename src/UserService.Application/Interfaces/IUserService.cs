using SharedLibrary.Dtos.Users;

namespace UserService.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAll();
    Task<UserDto?> GetUserByIdAsync(string id);
    Task<bool> CreateUserAsync(CreateUserDto input);
    Task<bool> UpdateUserAsync(string id, UpdateUserDto input);
    Task<bool> DeleteUserAsync(string id);
}