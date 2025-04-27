using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
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

    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "RedisCache";
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { options.Configuration }
            };
        });

        return services;
    }
}