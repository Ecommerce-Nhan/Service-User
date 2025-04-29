using Grpc.Core;
using gRPCServer.User.Protos;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserService.Domains.Entities;

namespace UserService.Application.GrpcServices;

public class UserGrpcService(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager)
           : UserProtoService.UserProtoServiceBase
{
    public override async Task<LoginModel> Login(LoginRequest request, ServerCallContext context)
    {
        var user = await userManager.FindByEmailAsync(request.Username);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found."));
        }
        var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid username or password."));
        }
        var result = await signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);
        if (!result.Succeeded)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid username or password."));
        }
        var roleUser = await userManager.GetRolesAsync(user);
        if (!roleUser.Any())
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, "User does not have any assigned roles."));
        }

        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();
        foreach (var role in roleUser)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role));
            var thisRole = await roleManager.FindByNameAsync(role);
            if (thisRole != null)
            {
                var allPermissionsForThisRoles = await roleManager.GetClaimsAsync(thisRole);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }
        }

        var response = new LoginModel();
        response.UserId = user.Id;
        response.UserRole = string.Join(", ", roleUser);
        response.UserPermission = string.Join(", ", permissionClaims.Select(x => x.Value).Distinct());

        return response;
    }
}