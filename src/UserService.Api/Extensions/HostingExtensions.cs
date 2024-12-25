using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedLibrary.Extentions;
using SharedLibrary.Repositories.Abtractions;
using System;
using UserService.Api.Extentions;
using UserService.Application.Interfaces;
using UserService.Application.Mappers;
using UserService.Entities.Abstractions;
using UserService.Infrastructure;
using UserService.Infrastructure.Repositories;
using MainService = UserService.Application.Implements.UserService;

namespace UserService.Api.Extensions;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddCustomIdentity();
        builder.Services.AddRedisCacheConfiguration();
        builder.Services.AddAutoMapper(typeof(UserAutoMapperProfile).Assembly);
        builder.Services.AddHandleException();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, MainService>();

        return builder.Build();
    }
    public static WebApplication ConfigurePipeline(this WebApplication app, WebApplicationBuilder builder)
    {
        app.MapUserEndpoints();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<UserDbContext>();
            context.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DisplayRequestDuration();
            });
        }

        app.UseExceptionHandler();
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();

        return app;
    }
}
