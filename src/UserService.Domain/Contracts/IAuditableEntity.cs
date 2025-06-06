﻿namespace UserService.Domains.Contracts;

public interface IAuditableEntity : IEntity
{
    string CreatedBy { get; set; }

    DateTime CreatedOn { get; set; }

    string LastModifiedBy { get; set; }

    DateTime? LastModifiedOn { get; set; }
}

public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId>;