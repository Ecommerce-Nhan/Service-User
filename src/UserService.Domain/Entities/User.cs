using Microsoft.AspNetCore.Identity;
using UserService.Domains.Contracts;

namespace UserService.Domains.Entities;

public class User : IdentityUser, IAuditableEntity<string>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public bool IsActive { get; set; }
    public bool IsDelete { get; set; }
    public User()
    {
        UserRoles = new HashSet<UserRole>();
    }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedOn { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}