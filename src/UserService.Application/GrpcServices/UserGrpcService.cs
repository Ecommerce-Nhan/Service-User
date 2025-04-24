using Grpc.Core;
using gRPCServer.User.Protos;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using UserService.Entities;

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
        var roles = roleUser.Select(role => new Claim(ClaimTypes.Role, role));

        var roleClaims = new List<Claim>();
        foreach (var role in roleUser)
        {
            var roleEntity = await roleManager.FindByNameAsync(role);
            if (roleEntity != null)
            {
                var claims = await roleManager.GetClaimsAsync(roleEntity);
                roleClaims.AddRange(claims);
            }
        }
        var response = new LoginModel();
        response.UserId = user.Id;
        response.UserRole.AddRange(roleUser);
        
        return response;
    }
}
