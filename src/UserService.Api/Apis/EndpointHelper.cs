namespace UserService.Api.Apis;

public static class EndpointHelper
{
    public static RouteHandlerBuilder WithNameAndSummary(this RouteHandlerBuilder builder, Delegate handler)
    {
        var name = handler.Method.Name;
        return builder.WithName(name).WithSummary(name);
    }
}
