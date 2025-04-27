using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using SharedLibrary.Exceptions;
using SharedLibrary.Response.Identity;
using SharedLibrary.Wrappers;
using System.Net;

namespace UserService.Application;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                Exception exception,
                                                CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception is BaseException e
                                          ? (int)e.StatusCode
                                          : (int)HttpStatusCode.InternalServerError;

        var response = await Response<PermissionResponse>.FailAsync(new List<string> { exception.Message });
        response.Message = "Something went wrong";
        Log.Error(exception.Message);
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken)
                                  .ConfigureAwait(false);

        return true;
    }
}