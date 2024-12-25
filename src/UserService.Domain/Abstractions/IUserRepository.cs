namespace UserService.Entities.Abstractions;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(string id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<bool> CreateUserAsync(User user);
}