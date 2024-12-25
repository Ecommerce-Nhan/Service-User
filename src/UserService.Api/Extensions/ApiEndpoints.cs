using Microsoft.AspNetCore.Mvc;
using UserService.Application.Interfaces;
using SharedLibrary.Dtos.Users;

namespace UserService.Api.Extensions;

public static class ApiEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/user", async (IUserService service) => await service.GetAll());
        app.MapGet("/user/{id}", async (string id, IUserService service) =>
        {
            return Results.Ok(await service.GetUserByIdAsync(id));
        });
        app.MapPost("/user", async ([FromBody] CreateUserDto input, IUserService service) =>
        {
            return Results.Ok(await service.CreateUserAsync(input));
        });
        app.MapDelete("/user/{id}", async (string id, IUserService db) =>
        {
            await Task.CompletedTask;
            return Results.NoContent();
        });
    }
}
