using SharedLibrary.Response.Identity;
using SharedLibrary.Wrappers;

namespace UserService.Application.Interfaces;

public interface IRoleClaimService
{
    Task<Response<List<RoleClaimResponse>>> GetAllAsync();

    Task<int> GetCountAsync();

    Task<Response<RoleClaimResponse>> GetByIdAsync(int id);

    Task<Response<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);
}