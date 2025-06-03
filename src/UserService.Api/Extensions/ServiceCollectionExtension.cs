using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserService.Api.Extensions;
using UserService.Domains.Entities;
using UserService.Infrastructure;
using UserService.Infrastructure.Seeds;

namespace UserService.Api.Extentions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
        {
            var connecstring = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connecstring);
        });

        return services;
    }

    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;

            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = true;

            options.Tokens.EmailConfirmationTokenProvider =
                "emailconfirmation";
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@#$%^&*()+/=<> ";
        })
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders()
        .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation");
        services.Configure<EmailConfirmationTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(1));
        services.Configure<DataProtectionTokenProviderOptions>(x => x.TokenLifespan = TimeSpan.FromDays(1));
        services.AddHostedService<SeedWorker>();

        return services;
    }

    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddApiVersioning(cfg =>
        {
            cfg.DefaultApiVersion = new ApiVersion(1, 0);
            cfg.AssumeDefaultVersionWhenUnspecified = true;
            cfg.ReportApiVersions = true;
            cfg.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
            cfg.UnsupportedApiVersionStatusCode = StatusCodes.Status400BadRequest;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "User API v1",
                Version = "v1",
                Description = "Development by TTNhan"
            });
        });

        return services;
    }
}