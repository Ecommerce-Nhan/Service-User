using UserService.Api.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddCustomIdentity();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/", () =>
{
    return "Hello World!";
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
