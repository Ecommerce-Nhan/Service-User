using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Requests.Identity;
using UserService.Application.Interfaces;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/user/role")]
[ApiVersion("1.0")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
        => Ok(await _roleService.GetAll());

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequest roleRequest)
        => Ok(await _roleService.CreateAsync(roleRequest));

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleRequest roleRequest)
        => Ok(await _roleService.UpdateAsync(roleRequest));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id)
        => Ok(await _roleService.DeleteAsync(id));

    [HttpGet("permissions/{roleId}")]
    public async Task<IActionResult> GetPermissionByRole(string roleId)
        => Ok(await _roleService.GetAllPermissionsAsync(roleId));

    [HttpPut("permissions/update")]
    public async Task<IActionResult> UpdatePermissions([FromBody] PermissionRequest permissionRequest)
        => Ok(await _roleService.UpdatePermissionsAsync(permissionRequest));
}