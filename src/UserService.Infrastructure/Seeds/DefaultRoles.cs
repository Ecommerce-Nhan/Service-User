﻿using Microsoft.AspNetCore.Identity;
using SharedLibrary.Helpers;
using UserService.Domains.Entities;

namespace UserService.Infrastructure.Seeds;

public static class DefaultRoles
{
    public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        await roleManager.CreateAsync(new Role(Roles.SuperAdmin.ToString()));
        await roleManager.CreateAsync(new Role(Roles.Admin.ToString()));
        await roleManager.CreateAsync(new Role(Roles.Basic.ToString()));
    }
}