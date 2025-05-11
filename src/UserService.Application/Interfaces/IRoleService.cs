using SharedLibrary.Requests.Identity;
using SharedLibrary.Response.Identity;
using SharedLibrary.Wrappers;

namespace UserService.Application.Interfaces;

public interface IRoleService
{
    Task<int> GetCountAsync();
    Task<Response<List<RoleResponse>>> GetAll();
    Task<Response<RoleResponse>> GetByIdAsync(string id);
    Task<Response<string>> CreateAsync(RoleRequest input);
    Task<Response<string>> UpdateAsync(RoleRequest request);
    Task<Response<string>> DeleteAsync(string id);

    Task<Response<PermissionResponse>> GetAllPermissionsAsync(string roleId);
}