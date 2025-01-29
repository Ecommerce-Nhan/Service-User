using UserService.Application.Interfaces;
using SharedLibrary.Dtos.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace UserService.Api.Extensions;

public static class ApiEndpointExtension
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        string endpoint = "/user";
        app.MapGet(endpoint, [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (IUserService service) =>
        await service.GetAll());

        app.MapGet($"{endpoint}/{{id}}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (string id, IUserService service) =>
            Results.Ok(await service.GetUserByIdAsync(id)));

        app.MapPost(endpoint, [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (CreateUserDto input, IUserService service) =>
            Results.Ok(await service.CreateUserAsync(input)));

        app.MapPut(endpoint, [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (string id, UpdateUserDto input, IUserService service) =>
            Results.Ok(await service.UpdateUserAsync(id, input)));

        app.MapDelete($"{endpoint}/{{id}}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (string id, IUserService service) =>
            Results.Ok(await service.DeleteUserAsync(id)));

        app.MapGet("/profile", (HttpContext httpContext) =>
        {
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                var userName = httpContext.User.Identity.Name;
                var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var roles = httpContext.User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();

                return Results.Ok(new { userName, userId, roles });
            }

            return Results.Unauthorized();
        });
    }
}