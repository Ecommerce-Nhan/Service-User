using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace UserService.Infrastructure.Seeds;

public class SeedWorker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public SeedWorker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("app");
        
        try
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            var role = "SuperAdmin";
            var permissions = new HashSet<string>
            {
                "Permissions.Users.Read",
                "Permissions.Users.Create",
                "Permissions.Products.Read",
                "Permissions.Products.Create"
            };
            var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            await cache.SetStringAsync($"role_permissions:{role}", JsonSerializer.Serialize(permissions));

            await Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
            await Seeds.DefaultUsers.SeedBasicUserAsync(userManager, roleManager);
            await Seeds.DefaultUsers.SeedSuperAdminAsync(userManager, roleManager);
            logger.LogInformation("Finished Seeding Default Data");
            logger.LogInformation("Application Starting");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "An error occurred seeding the DB");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
