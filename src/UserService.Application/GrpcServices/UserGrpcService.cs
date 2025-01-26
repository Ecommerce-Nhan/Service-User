using Grpc.Core;
using gRPCServer.User.Protos;
using Microsoft.AspNetCore.Identity;
using UserService.Entities;

namespace UserService.Application.GrpcServices;

public class UserGrpcService(SignInManager<User> signInManager, UserManager<User> userManager) 
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
        //if (!roleUser.Any())
        //{
        //    throw new RpcException(new Status(StatusCode.PermissionDenied, "User does not have any assigned roles."));
        //}
        var response = new LoginModel();
        response.UserId = user.Id;
        response.UserRole = "roleUser.FirstOrDefault()";

        return response;
    }
}
