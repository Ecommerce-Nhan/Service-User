namespace UserService.Domain.Users;

public interface IUserRepository
{
    Task<UserDomain?> GetUserById(string id);
    Task CreateUser(UserDomain user, string password);
}