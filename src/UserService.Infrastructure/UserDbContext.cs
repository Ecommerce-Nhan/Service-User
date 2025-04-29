using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Domains.Entities;

namespace UserService.Infrastructure;

public class UserDbContext : IdentityDbContext<User, Role, string,
                                               IdentityUserClaim<string>,
                                               UserRole,
                                               IdentityUserLogin<string>,
                                               RoleClaim,
                                               IdentityUserToken<string>>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName) && tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
    }
}
