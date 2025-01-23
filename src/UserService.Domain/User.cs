using Microsoft.AspNetCore.Identity;

namespace UserService.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public bool IsActive { get; set; }
    public bool CreateDate { get; set; }
    public bool UpdateDate { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public string DeleteBy { get; set; } = string.Empty;
}