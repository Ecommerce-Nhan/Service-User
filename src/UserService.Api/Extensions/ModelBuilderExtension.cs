using Microsoft.EntityFrameworkCore;
using UserService.Entities;

namespace UserService.Api.Extensions;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var roleIdAdmin = new string("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
        var roleIdClient = new string("1B3D7E19-B1A5-4CA2-A491-54593FA16531");
        var roleIdEmployee = new string("5603EB1E-A44D-4C72-9BD5-6546BB750045");
        modelBuilder.Entity<Role>().HasData(new Role
        {
            Id = roleIdAdmin,
            Name = "Admin",
            NormalizedName = "ADMIN"
        },
        new Role
        {
            Id = roleIdClient,
            Name = "Client",
            NormalizedName = "CLIENT"
        },
        new Role
        {
            Id = roleIdEmployee,
            Name = "Employee",
            NormalizedName = "EMPLOYEE"
        });
    }
}
