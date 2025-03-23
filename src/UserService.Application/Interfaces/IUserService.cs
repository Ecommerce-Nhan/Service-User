using SharedLibrary.Dtos.Users;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace UserService.Application.Interfaces;

public interface IUserService
{
    Task<PagedResponse<List<UserDto>>> GetAll(PaginationFilter pagination);
    Task<UserDto?> GetUserByIdAsync(string id);
    Task<bool> CreateUserAsync(CreateUserDto input);
    Task<bool> UpdateUserAsync(string id, UpdateUserDto input);
    Task<bool> DeleteUserAsync(string id);
}