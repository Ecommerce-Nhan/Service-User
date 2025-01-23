using Microsoft.AspNetCore.Identity;

namespace UserService.Entities;

public class Role : IdentityRole
{
    public Role() : base()
    {
    }
    public Role(string roleName) : base(roleName)
    {
    }
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
