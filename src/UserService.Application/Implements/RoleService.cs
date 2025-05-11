using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Constants.Role;
using SharedLibrary.Requests.Identity;
using SharedLibrary.Response.Identity;
using SharedLibrary.Wrappers;
using UserService.Application.Interfaces;
using UserService.Domains.Entities;
using static UserService.Infrastructure.Helpers.ClaimsHelper;

namespace UserService.Application.Implements;

public class RoleService(RoleManager<Role> roleManager,
    UserManager<User> userManager,
    IRoleClaimService roleClaimService,
    IMapper mapper) : IRoleService
{
    public async Task<int> GetCountAsync()
    {
        var count = await roleManager.Roles.CountAsync();
        return count;
    }

    public async Task<Response<string>> CreateAsync(RoleRequest request)
    {
        var existingRole = await roleManager.FindByNameAsync(request.Name);
        if (existingRole != null) return await Response<string>.FailAsync("Similar Role already exists.");
        var response = await roleManager.CreateAsync(new Role(request.Name, request.Description));
        if (response.Succeeded)
        {
            return await Response<string>.SuccessAsync(string.Format("Role {0} Created.", request.Name));
        }
        else
        {
            return await Response<string>.FailAsync(response.Errors.Select(e => e.Description.ToString()).ToList());
        }
    }

    public async Task<Response<string>> DeleteAsync(string id)
    {
        var existingRole = await roleManager.FindByIdAsync(id);

        if (existingRole is not null && existingRole.Name != RoleConstants.AdministratorRole &&
            existingRole.Name != RoleConstants.BasicRole)
        {
            bool roleIsNotUsed = true;
            var allUsers = await userManager.Users.ToListAsync();
            foreach (var user in allUsers)
            {
                if (await userManager.IsInRoleAsync(user, existingRole.Name))
                {
                    roleIsNotUsed = false;
                }
            }
            if (roleIsNotUsed)
            {
                await roleManager.DeleteAsync(existingRole);
                return await Response<string>.SuccessAsync(string.Format("Role {0} Deleted.", existingRole.Name));
            }
            else
            {
                return await Response<string>.SuccessAsync(string.Format("Not allowed to delete {0} Role as it is being used.", existingRole.Name));
            }
        }
        else
        {
            return await Response<string>.SuccessAsync(string.Format("Not allowed to delete {0} Role.", existingRole.Name));
        }
    }

    public async Task<Response<List<RoleResponse>>> GetAll()
    {
        var roles = await roleManager.Roles.ToListAsync();
        var rolesResponse = mapper.Map<List<RoleResponse>>(roles);
        return await Response<List<RoleResponse>>.SuccessAsync(rolesResponse);
    }

    public Task<Response<RoleResponse>> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<string>> UpdateAsync(RoleRequest request)
    {
        var existingRole = await roleManager.FindByIdAsync(request.Id);
        if (existingRole == null) return await Response<string>.FailAsync("Role not found.");
        if (existingRole.Name == RoleConstants.AdministratorRole || existingRole.Name == RoleConstants.BasicRole)
        {
            return await Response<string>.FailAsync(string.Format("Not allowed to modify {0} Role.", existingRole.Name));
        }
        existingRole.Name = request.Name;
        existingRole.NormalizedName = request.Name.ToUpper();
        existingRole.Description = request.Description;
        await roleManager.UpdateAsync(existingRole);
        return await Response<string>.SuccessAsync(string.Format("Role {0} Updated.", existingRole.Name));
    }

    public async Task<Response<PermissionResponse>> GetAllPermissionsAsync(string roleId)
    {
        var model = new PermissionResponse();
        var allPermissions = GetAllPermissions();
        var role = await roleManager.FindByIdAsync(roleId);
        if (role != null && !string.IsNullOrEmpty(role.Name))
        {
            model.RoleId = role.Id;
            model.RoleName = role.Name;
            var roleClaimsResult = await roleClaimService.GetAllByRoleIdAsync(role.Id);
            if (roleClaimsResult.Succeeded)
            {
                var roleClaims = roleClaimsResult.Data;
                var allClaimValues = allPermissions.Select(a => a.Value).ToList();
                var roleClaimValues = roleClaims.Select(a => a.Value).ToList();
                var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
                foreach (var permission in allPermissions)
                {
                    if (authorizedClaims.Any(a => a == permission.Value))
                    {
                        permission.Selected = true;
                        var roleClaim = roleClaims.SingleOrDefault(a => a.Value == permission.Value);
                        if (roleClaim?.Description != null)
                        {
                            permission.Description = roleClaim.Description;
                        }
                        if (roleClaim?.Group != null)
                        {
                            permission.Group = roleClaim.Group;
                        }
                    }
                }
            }
            else
            {
                model.RoleClaims = new List<RoleClaimResponse>();
                return await Response<PermissionResponse>.FailAsync(roleClaimsResult.Errors);
            }
        }
        model.RoleClaims = allPermissions;
        return await Response<PermissionResponse>.SuccessAsync(model);
    }

    private List<RoleClaimResponse> GetAllPermissions()
    {
        var allPermissions = new List<RoleClaimResponse>();

        #region GetPermissions

        allPermissions.GetAllPermissions();

        #endregion GetPermissions

        return allPermissions;
    }
}