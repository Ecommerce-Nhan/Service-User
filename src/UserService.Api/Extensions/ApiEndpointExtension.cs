using UserService.Application.Interfaces;
using SharedLibrary.Dtos.Users;

namespace UserService.Api.Extensions;

public static class ApiEndpointExtension
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        string endpoint = "/user";
        app.MapGet(endpoint, async (IUserService service) => 
            await service.GetAll());

        app.MapGet($"{endpoint}/{{id}}", async (string id, IUserService service) =>
            Results.Ok(await service.GetUserByIdAsync(id)));

        app.MapPost(endpoint, async (CreateUserDto input, IUserService service) =>
            Results.Ok(await service.CreateUserAsync(input)));

        app.MapPut(endpoint, async (string id, UpdateUserDto input, IUserService service) =>
            Results.Ok(await service.UpdateUserAsync(id, input)));

        app.MapDelete($"{endpoint}/{{id}}", async (string id, IUserService service) =>
            Results.Ok(await service.DeleteUserAsync(id)));
    }
}