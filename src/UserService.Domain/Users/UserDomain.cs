namespace UserService.Domain.Users;

public class UserDomain
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public bool IsActive { get; set; }
    public UserDomain()
    {
    }
    public UserDomain(string username, string email)
    {
        Id = Guid.NewGuid().ToString();
        Username = username;
        Email = email;
    }
    // Domain-specific methods
}