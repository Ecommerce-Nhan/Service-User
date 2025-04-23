using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos.Users;
using SharedLibrary.Filters;
using UserService.Application.Interfaces;
using UserService.Entities;

namespace UserService.Api.Extensions;

public static class ApiEndpointExtension
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        string endpoint = "/role";
        app.MapGet(endpoint, [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (RoleManager<Role> roleManager) =>
                   await roleManager.Roles.ToListAsync());
    }

    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder builder)
    {
        var vApi = builder.NewVersionedApi("UserService");
        var v1 = vApi
            .MapGroup("api/v{version:apiVersion}/user")
            .HasApiVersion(1, 0)
            .RequireAuthorization(JwtBearerDefaults.AuthenticationScheme);

        v1.MapGet("/", GetAllUsers).WithNameAndSummary(GetAllUsers);
        v1.MapGet("/{id}", GetUserById).WithNameAndSummary(GetUserById);
        v1.MapPost("/", CreateUser).WithNameAndSummary(CreateUser);
        v1.MapPut("/", UpdateUser).WithNameAndSummary(UpdateUser);
        v1.MapDelete("/{id}", DeleteUser).WithNameAndSummary(DeleteUser);

        return builder;
    }

    private static RouteHandlerBuilder WithNameAndSummary(this RouteHandlerBuilder builder, Delegate handler)
    {
        var name = handler.Method.Name;
        return builder.WithName(name).WithSummary(name);
    }

    private static async Task<IResult> GetAllUsers(IUserService service, [AsParameters] PaginationFilter pagination)
    {
        var result = await service.GetAll(pagination);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetUserById(string id, IUserService service)
    {
        var result = await service.GetUserByIdAsync(id);
        return Results.Ok(result);
    }

    private static async Task<IResult> CreateUser(CreateUserDto input, IUserService service)
    {
        var result = await service.CreateUserAsync(input);
        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateUser(string id, UpdateUserDto input, IUserService service)
    {
        var result = await service.UpdateUserAsync(id, input);
        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteUser(string id, IUserService service)
    {
        var result = await service.DeleteUserAsync(id);
        return Results.Ok(result);
    }
}