using SharedLibrary.Requests.Identity;
using UserService.Application.Interfaces;

namespace UserService.Api.Apis;

public static partial class ApiEndpointExtension
{
    public static IEndpointRouteBuilder MapRoleClaimEndpoints(this IEndpointRouteBuilder builder)
    {
        var vApi = builder.NewVersionedApi("RoleClaim");
        var v1 = vApi.MapGroup("api/v{version:apiVersion}/user/role-claim")
                     .HasApiVersion(1, 0);

        v1.MapGet("/", GetRoleClaims).WithNameAndSummary(GetRoleClaims);
        v1.MapPost("/", CreateRoleClaim).WithNameAndSummary(CreateRoleClaim);
        v1.MapPut("/{id:int}", UpdateRoleClaim).WithNameAndSummary(UpdateRoleClaim);
        v1.MapDelete("/{id:int}", DeleteRoleClaim).WithNameAndSummary(DeleteRoleClaim);
        v1.MapGet("/by-role/{roleId}", GetAllByRoleId).WithNameAndSummary(GetAllByRoleId);

        return builder;
    }

    private static async Task<IResult> GetRoleClaims(IRoleClaimService roleClaimService)
        => Results.Ok(await roleClaimService.GetAllAsync());

    private static async Task<IResult> CreateRoleClaim(RoleClaimRequest roleRequest, IRoleClaimService roleClaimService)
        => Results.Ok(await roleClaimService.CreateAsync(roleRequest));

    private static async Task<IResult> UpdateRoleClaim(int id, RoleClaimRequest roleRequest, IRoleClaimService roleClaimService)
        => Results.Ok(await roleClaimService.UpdateAsync(id, roleRequest));

    private static async Task<IResult> DeleteRoleClaim(int id, IRoleClaimService roleClaimService)
        => Results.Ok(await roleClaimService.DeleteAsync(id));

    private static async Task<IResult> GetAllByRoleId(string roleId, IRoleClaimService roleClaimService)
        => Results.Ok(await roleClaimService.GetAllByRoleIdAsync(roleId));
}
