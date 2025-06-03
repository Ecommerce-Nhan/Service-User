using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Requests.Identity;
using UserService.Application.Interfaces;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/user/role-claim")]
[ApiVersion("1.0")]
public class RoleClaimController : ControllerBase
{
    private readonly IRoleClaimService _roleClaimService;

    public RoleClaimController(IRoleClaimService roleClaimService)
    {
        _roleClaimService = roleClaimService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoleClaims()
        => Ok(await _roleClaimService.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> CreateRoleClaim([FromBody] RoleClaimRequest roleRequest)
        => Ok(await _roleClaimService.CreateAsync(roleRequest));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRoleClaim(int id, [FromBody] RoleClaimRequest roleRequest)
        => Ok(await _roleClaimService.UpdateAsync(id, roleRequest));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRoleClaim(int id)
        => Ok(await _roleClaimService.DeleteAsync(id));

    [HttpGet("by-role/{roleId}")]
    public async Task<IActionResult> GetAllByRoleId(string roleId)
        => Ok(await _roleClaimService.GetAllByRoleIdAsync(roleId));
}