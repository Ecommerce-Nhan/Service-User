using UserService.Api.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddCustomIdentity();

var app = builder.Build();

app.MapGet("/", () =>
{
    return "Hello World!";
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Run();
