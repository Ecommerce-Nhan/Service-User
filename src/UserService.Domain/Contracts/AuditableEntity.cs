namespace UserService.Domains.Contracts;

public abstract class AuditableEntity<TId> : IAuditableEntity<TId>
{
    public TId Id { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
    public DateTime? LastModifiedOn { get; set; }

    public void UpdateLastModified()
    {
        LastModifiedOn = DateTime.UtcNow;
    }
}