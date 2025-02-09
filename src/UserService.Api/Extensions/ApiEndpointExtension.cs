using UserService.Application.Interfaces;
using SharedLibrary.Dtos.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using UserService.Entities;
using Microsoft.EntityFrameworkCore;

namespace UserService.Api.Extensions;

public static class ApiEndpointExtension
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        string endpoint = "/user";
        var userGroup = app.MapGroup(endpoint).RequireAuthorization(JwtBearerDefaults.AuthenticationScheme);
        userGroup.MapGet("/", async (IUserService service) => await service.GetAll());

        userGroup.MapGet("/{id}", async (string id, IUserService service) =>
            Results.Ok(await service.GetUserByIdAsync(id)));

        userGroup.MapPost("/", async (CreateUserDto input, IUserService service) =>
            Results.Ok(await service.CreateUserAsync(input)));

        userGroup.MapPut("/", async (string id, UpdateUserDto input, IUserService service) =>
            Results.Ok(await service.UpdateUserAsync(id, input)));

        userGroup.MapDelete("/{id}", async (string id, IUserService service) =>
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

    public static void MapRoleEndpoints(this WebApplication app)
    {
        string endpoint = "/role";
        app.MapGet(endpoint, [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (RoleManager<Role> roleManager) =>
                   await roleManager.Roles.ToListAsync());
    }
}