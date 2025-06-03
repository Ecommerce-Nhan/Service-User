using Microsoft.AspNetCore.Identity;
using UserService.Domains.Contracts;

namespace UserService.Domains.Entities;

public class RoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
{
    public string? Description { get; set; }
    public string? Group { get; set; }
    public RoleClaim() : base()
    {
    }

    public RoleClaim(string? roleClaimDescription = null, string? roleClaimGroup = null) : base()
    {
        Description = roleClaimDescription;
        Group = roleClaimGroup;
    }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedOn { get; set; }
    public virtual Role Role { get; set; }
}