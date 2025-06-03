using Microsoft.AspNetCore.Identity;
using UserService.Domains.Contracts;

namespace UserService.Domains.Entities;

public class Role : IdentityRole, IAuditableEntity
{
    public string? Description { get; set; }
    public Role() : base()
    {
        RoleClaims = new HashSet<RoleClaim>();
    }
    public Role(string roleName, string? roleDescription = null) : base(roleName)
    {
        UserRoles = new HashSet<UserRole>();
        RoleClaims = new HashSet<RoleClaim>();
        Description = roleDescription;
    }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedOn { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<RoleClaim> RoleClaims { get; set; }
}