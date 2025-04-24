using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;
using SharedLibrary.Exceptions;
using SharedLibrary.Wrappers;
using System.Net;

namespace UserService.Application;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                Exception exception,
                                                CancellationToken cancellationToken)
    {
        var response = new Response<object>();
        response.Succeeded = false;
        response.Data = default!;
        response.Message = "Something went wrong";
        response.Errors = new[] { exception.Message };
        httpContext.Response.StatusCode = exception is BaseException e
                                          ? (int)e.StatusCode
                                          : (int)HttpStatusCode.InternalServerError;
        Log.Error(exception.Message);
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken)
                                  .ConfigureAwait(false);

        return true;
    }
}