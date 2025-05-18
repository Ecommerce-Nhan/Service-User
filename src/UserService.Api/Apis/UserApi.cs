using SharedLibrary.Dtos.Users;
using SharedLibrary.Filters;
using SharedLibrary.Requests.Identity;
using UserService.Application.Interfaces;

namespace UserService.Api.Apis;

public static partial class ApiEndpointExtension
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var vApi = builder.NewVersionedApi("User");
        var v1 = vApi.MapGroup("api/v{version:apiVersion}/user")
                     .HasApiVersion(1, 0);

        v1.MapGet("/", GetUsers).WithNameAndSummary(GetUsers);
        v1.MapGet("/{id}", GetUserById).WithNameAndSummary(GetUserById);
        v1.MapPost("/", CreateUser).WithNameAndSummary(CreateUser);
        v1.MapPut("/", UpdateUser).WithNameAndSummary(UpdateUser);
        v1.MapDelete("/{id}", DeleteUser).WithNameAndSummary(DeleteUser);

        v1.MapGet("/user-role/{userId}", GetUserRoles).WithNameAndSummary(GetUserRoles);
        v1.MapPut("/user-role/{userId}", UpdateUserRoles).WithNameAndSummary(UpdateUserRoles);

        return builder;
    }

    private static async Task<IResult> GetUsers(IUserService service)
        => Results.Ok(await service.GetAll(new PaginationFilter()));

    private static async Task<IResult> GetUserById(string id, IUserService service)
        => Results.Ok(await service.GetUserByIdAsync(id));

    private static async Task<IResult> CreateUser(RegisterRequest input, IUserService service)
        => Results.Ok(await service.CreateUserAsync(input));

    private static async Task<IResult> UpdateUser(string id, UpdateUserDto input, IUserService service)
        => Results.Ok(await service.UpdateUserAsync(id, input));

    private static async Task<IResult> DeleteUser(string id, IUserService service)
        => Results.Ok(await service.DeleteUserAsync(id));

    private static async Task<IResult> GetUserRoles(string userId, IUserService service)
        => Results.Ok(await service.GetRolesAsync(userId));

    private static async Task<IResult> UpdateUserRoles(string userId, UpdateUserRoleRequest request, IUserService service)
        => Results.Ok(await service.UpdateRolesAsync(request));
}