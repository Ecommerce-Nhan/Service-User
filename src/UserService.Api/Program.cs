using Microsoft.EntityFrameworkCore;
using SharedLibrary.Extentions;
using UserService.Api.Extensions;
using UserService.Api.Extentions;
using UserService.Application.Interfaces;
using UserService.Application.Mappers;
using UserService.Domain.Abstractions;
using UserService.Entities.Abstractions;
using UserService.Infrastructure;
using UserService.Infrastructure.Repositories;
using MainService = UserService.Application.Implements.UserService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddCustomIdentity();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, MainService>();
builder.Services.AddRedisCacheConfiguration();
builder.Services.AddAutoMapper(typeof(ApplicationAutoMapperProfile).Assembly);

var app = builder.Build();

app.MapUserEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UserDbContext>();
    context.Database.Migrate();
}

app.Run();
