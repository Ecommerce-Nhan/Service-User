using FluentValidation;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Orchestration.ServiceDefaults;
using Serilog;
using SharedLibrary.Dtos.Users;
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
        builder.Services.AddCustomIdentity();
        builder.AddServiceDefaults();

        builder.Host.UseSerilog();

        builder.Services.AddControllers();
        builder.Services.AddDatabaseConfiguration(builder.Configuration);
        builder.Services.AddSwaggerConfiguration();
        builder.Services.AddAutoMapper(typeof(UserAutoMapperProfile).Assembly);
        builder.Services.AddGrpc();

        builder.Services.AddScoped<IUserService, UserMainService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IRoleClaimService, RoleClaimService>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();

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
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DisplayRequestDuration();
            });
            app.UseHangfireDashboard();
        }
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        app.MapGrpcService<UserGrpcService>();
        app.MapControllers();

        return app;
    }
}