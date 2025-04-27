using Microsoft.AspNetCore.Identity;
using UserService.Application.Interfaces;
using UserService.Domains.Entities;

namespace UserService.Api.Apis;

public static partial class ApiEndpointExtension
{
    public static IEndpointRouteBuilder MapRoleEndpoints(this IEndpointRouteBuilder builder)
    {
        var vApi = builder.NewVersionedApi("Role");
        var v1 = vApi.MapGroup("api/v{version:apiVersion}/user/role")
                     .HasApiVersion(1, 0);

        v1.MapGet("/", GetRoles).WithNameAndSummary(GetRoles);
        v1.MapPost("/", CreateRole).WithNameAndSummary(CreateRole);

        return builder;
    }

    private static async Task<IResult> GetRoles(IRoleService roleService)
        => Results.Ok(await roleService.GetAll());

    private static async Task<IResult> CreateRole(string roleName, RoleManager<Role> roleManager)
        => Results.Ok(await roleManager.CreateAsync(new Role(roleName.Trim())));

}