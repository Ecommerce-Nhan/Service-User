using Microsoft.AspNetCore.Authentication.JwtBearer;
using SharedLibrary.Dtos.Users;
using SharedLibrary.Filters;
using System.Security.Claims;
using UserService.Application.Interfaces;

namespace UserService.Api.Apis;

public static partial class ApiEndpointExtension
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var vApi = builder.NewVersionedApi("User");
        var v1 = vApi.MapGroup("api/v{version.apiVersion}/user")
                     .RequireAuthorization(JwtBearerDefaults.AuthenticationScheme)
                     .HasApiVersion(1, 0);

        v1.MapGet("/", GetUsers);
        v1.MapPost("/", CreateUser);
        v1.MapGet("/{id}", GetUserById);
        v1.MapPut("/{id}", UpdateUser);
        v1.MapDelete("/{id}", DeleteUser);

        return builder;
    }

    private static async Task<IResult> GetUsers(
        IUserService service,
        [AsParameters] PaginationFilter pagination)
        => Results.Ok(await service.GetAll(pagination));

    private static async Task<IResult> GetUserById(string id, IUserService service)
        => Results.Ok(await service.GetUserByIdAsync(id));

    private static async Task<IResult> CreateUser(CreateUserDto input, IUserService service)
        => Results.Ok(await service.CreateUserAsync(input));

    private static async Task<IResult> UpdateUser(string id, UpdateUserDto input, IUserService service)
        => Results.Ok(await service.UpdateUserAsync(id, input));

    private static async Task<IResult> DeleteUser(string id, IUserService service)
        => Results.Ok(await service.DeleteUserAsync(id));
}
