using Asp.Versioning;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Orchestration.ServiceDefaults;
using Scalar.AspNetCore;
using Serilog;
using SharedLibrary.Dtos.Users;
using UserService.Api.Apis;
using UserService.Api.Extentions;
using UserService.Application.GrpcServices;
using UserService.Application.Implements;
using UserService.Application.Interfaces;
using UserService.Application.Mappers;
using UserService.Application.Validations;
using UserService.Infrastructure;
using UserMainService = UserService.Application.Implements.UserService;

namespace UserService.Api.Extensions;

internal static class HostingExtensions
{
    
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Host.UseSerilog();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddCustomIdentity();
        builder.Services.AddAutoMapper(typeof(UserAutoMapperProfile).Assembly);
        builder.Services.AddGrpc();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddScoped<IUserService, UserMainService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IRoleClaimService, RoleClaimService>();
        builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
        builder.Services.AddOpenApi();
        builder.Services.AddApiVersioning(
            opts =>
            {
                opts.ReportApiVersions = true;
                opts.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("X-Version")
                );
            }
        );

        return builder.Build();
    }
    public static WebApplication ConfigurePipeline(this WebApplication app, WebApplicationBuilder builder)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<UserDbContext>();
            context.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                .WithTheme(ScalarTheme.Kepler)
                .WithDarkModeToggle(true)
                .WithClientButton(true)
                .WithTitle("User service API");
            });
        }

        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        app.MapGrpcService<UserGrpcService>();
        app.MapUserEndpoints();
        app.MapRoleEndpoints();

        return app;
    }
}