using SharedLibrary.Requests.Identity;
using SharedLibrary.Response.Identity;
using SharedLibrary.Wrappers;

namespace UserService.Application.Interfaces;

public interface IRoleClaimService
{
    Task<Response<List<RoleClaimResponse>>> GetAllAsync();

    Task<int> GetCountAsync();

    Task<Response<RoleClaimResponse>> GetByIdAsync(int id);

    Task<Response<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

    Task<Response<string>> CreateAsync(RoleClaimRequest request);

    Task<Response<string>> UpdateAsync(int id, RoleClaimRequest request);

    Task<Response<string>> DeleteAsync(int id);
}