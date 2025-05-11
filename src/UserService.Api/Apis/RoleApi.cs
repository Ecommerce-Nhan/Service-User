using SharedLibrary.Requests.Identity;
using UserService.Application.Interfaces;

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
        v1.MapPut("/{id}", UpdateRole).WithNameAndSummary(UpdateRole);
        v1.MapDelete("/{id}", DeleteRole).WithNameAndSummary(DeleteRole);

        return builder;
    }

    private static async Task<IResult> GetRoles(IRoleService roleService)
        => Results.Ok(await roleService.GetAll());

    private static async Task<IResult> CreateRole(RoleRequest roleRequest, IRoleService roleService)
        => Results.Ok(await roleService.CreateAsync(roleRequest));

    private static async Task<IResult> UpdateRole(string id, RoleRequest roleRequest, IRoleService roleService)
        => Results.Ok(await roleService.UpdateAsync(roleRequest));

    private static async Task<IResult> DeleteRole(string id, IRoleService roleService)
        => Results.Ok(await roleService.DeleteAsync(id));
}