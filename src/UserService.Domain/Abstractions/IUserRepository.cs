using SharedLibrary.Dtos.Users;

namespace UserService.Entities.Abstractions;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(string id);
    Task<bool> CreateUserAsync(User user);
}