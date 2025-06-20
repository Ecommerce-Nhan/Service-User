using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Orchestration.ServiceDefaults;
using Serilog;
using SharedLibrary.Dtos.HealthChecks;
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
        builder.Services.AddHealthChecks().AddDbContextCheck<UserDbContext>();

        return builder.Build();
    }
    public static WebApplication ConfigurePipeline(this WebApplication app, WebApplicationBuilder builder)
    {
        app.CheckHealthy();
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<UserDbContext>();
            context.Database.Migrate();
        }

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DisplayRequestDuration();
            });
            app.UseHangfireDashboard();
        }
        app.UseRouting();
        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        app.MapGrpcService<UserGrpcService>();
        app.MapControllers();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    private static WebApplication CheckHealthy(this WebApplication app)
    {
        app.UseHealthChecks("/api/v1/user/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var response = new HealthCheckResponse
                {
                    Status = report.Status.ToString(),
                    HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
                    {
                        Component = x.Key,
                        Status = x.Value.Status.ToString(),
                        Description = x.Value.Description ?? string.Empty

                    }),
                    HealthCheckDuration = report.TotalDuration
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        });

        return app;
    }
}