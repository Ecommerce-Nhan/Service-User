﻿namespace UserService.Domains.Contracts;

public interface IEntity<TId> : IEntity
{
    public TId Id { get; set; }
}
public interface IEntity;