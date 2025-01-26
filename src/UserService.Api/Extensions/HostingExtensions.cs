using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Debugging;
using SharedLibrary.Dtos.Users;
using SharedLibrary.Exceptions;
using SharedLibrary.Extentions;
using SharedLibrary.Repositories.Abtractions;
using UserService.Api.Extentions;
using UserService.Application.GrpcServices;
using UserService.Application.Interfaces;
using UserService.Application.Mappers;
using UserService.Application.Validations;
using UserService.Entities.Abstractions;
using UserService.Infrastructure;
using UserService.Infrastructure.Repositories;
using MainService = UserService.Application.Implements.UserService;

namespace UserService.Api.Extensions;

internal static class HostingExtensions
{
    public static void ConfigureSerilog(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                                                       .CreateLogger();

        SelfLog.Enable(msg => Log.Information(msg));
        Log.Information("Starting server.");
    }
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddCustomIdentity();
        builder.Services.AddRedisCacheConfiguration();
        builder.Services.AddAutoMapper(typeof(UserAutoMapperProfile).Assembly);
        builder.Services.AddGrpc();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, MainService>();
        builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();

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

        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.MapGrpcService<UserGrpcService>();

        return app;
    }
}
