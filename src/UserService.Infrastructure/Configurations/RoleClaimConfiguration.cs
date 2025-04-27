using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domains.Entities;

namespace UserService.Infrastructure.Configurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.HasKey(x => new { x.Id });
        builder.HasOne(rc => rc.Role)
               .WithMany(u => u.RoleClaims)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();
    }
}