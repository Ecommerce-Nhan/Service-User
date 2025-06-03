using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Orchestration.ServiceDefaults.Authorize;
using SharedLibrary.Constants.Permission;
using SharedLibrary.Dtos.Users;
using SharedLibrary.Filters;
using SharedLibrary.Requests.Identity;
using UserService.Application.Interfaces;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/user")]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    [PermissionAuthorize(Permissions.Users.View)]
    public async Task<IActionResult> GetUsers()
        => Ok(await _service.GetAll(new PaginationFilter()));

    [HttpGet("{id}")]
    [PermissionAuthorize(Permissions.Users.View)]
    public async Task<IActionResult> GetUserById(string id)
        => Ok(await _service.GetUserByIdAsync(id));

    [HttpPost]
    [PermissionAuthorize(Permissions.Users.Create)]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequest input)
        => Ok(await _service.CreateUserAsync(input));

    [HttpPut("{id}")]
    [PermissionAuthorize(Permissions.Users.Edit)]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto input)
        => Ok(await _service.UpdateUserAsync(id, input));

    [HttpDelete("{id}")]
    [PermissionAuthorize(Permissions.Users.Delete)]
    public async Task<IActionResult> DeleteUser(string id)
        => Ok(await _service.DeleteUserAsync(id));

    [HttpGet("user-role/{userId}")]
    [PermissionAuthorize(Permissions.Users.View)]
    public async Task<IActionResult> GetUserRoles(string userId)
        => Ok(await _service.GetRolesAsync(userId));

    [HttpPut("user-role/{userId}")]
    [PermissionAuthorize(Permissions.Users.Edit)]
    public async Task<IActionResult> UpdateUserRoles(string userId, [FromBody] UpdateUserRoleRequest request)
        => Ok(await _service.UpdateRolesAsync(request));
}