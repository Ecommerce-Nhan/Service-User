using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserService.Entities;
using UserService.Infrastructure.Constants;

namespace UserService.Infrastructure.Seeds;

public static class DefaultUsers
{
    public static async Task SeedBasicUserAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        var defaultUser = new User
        {
            UserName = "basicuser@gmail.com",
            Email = "basicuser@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            IsActive = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
            }
        }
    }
    public static async Task SeedSuperAdminAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        var defaultUser = new User
        {
            UserName = "superadmin@gmail.com",
            Email = "superadmin@gmail.com",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            IsActive = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
            }
            await roleManager.SeedClaimsForSuperAdmin();
        }
    }
    private async static Task SeedClaimsForSuperAdmin(this RoleManager<Role> roleManager)
    {
        var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
        if (adminRole is Role)
        {
            await roleManager.AddPermissionClaim(adminRole, "Product");
            await roleManager.AddPermissionClaim(adminRole, "User");
            await roleManager.AddPermissionClaim(adminRole, "Role");
            await roleManager.AddPermissionClaim(adminRole, "RoleClaim");
        }
    }
    public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string module)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        var allPermissions = Permissions.GeneratePermissionsForModule(module);
        foreach (var permission in allPermissions)
        {
            if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}