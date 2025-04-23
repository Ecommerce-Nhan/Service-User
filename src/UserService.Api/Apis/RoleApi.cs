using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Entities;

namespace UserService.Api.Apis;

public static partial class ApiEndpointExtension
{
    public static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder builder)
    {
        var vApi = builder.NewVersionedApi("User");
        var v1 = vApi.MapGroup("api/v{version.apiVersion}/user/role")
                     .RequireAuthorization(JwtBearerDefaults.AuthenticationScheme)
                     .HasApiVersion(1, 0);
        v1.MapGet("/", GetRoles);

        return builder;
    }

    private static async Task GetRoles(RoleManager<Role> roleManager)
        => await roleManager.Roles.ToListAsync();
}