using UserService.Entities.Users;

namespace UserService.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetUserById(string id);
    Task<bool> CreateUser(User user, string password);
}