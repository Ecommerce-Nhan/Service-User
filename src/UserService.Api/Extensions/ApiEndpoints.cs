using Microsoft.AspNetCore.Mvc;
using UserService.Application.Interfaces;
using SharedLibrary.Dtos.Users;

namespace UserService.Api.Extensions;

public static class ApiEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/user", async (IUserService service) => 
            await service.GetAll());

        app.MapGet("/user/{id}", async (string id, IUserService service) =>
            Results.Ok(await service.GetUserByIdAsync(id)));

        app.MapPost("/user", async (CreateUserDto input, IUserService service) =>
            Results.Ok(await service.CreateUserAsync(input)));

        app.MapPut("/user", async (string id, UpdateUserDto input, IUserService service) =>
            Results.Ok(await service.UpdateUserAsync(id, input)));

        app.MapDelete("/user/{id}", async (string id, IUserService service) =>
            Results.Ok(await service.DeleteUserAsync(id)));
    }
}
