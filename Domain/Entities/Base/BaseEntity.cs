using System;
using Domain.Interfaces;

namespace Domain.Entities.Base;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void SetCreatedAt(IDateTimeProvider dateTimeProvider)
    {
        CreatedAt = dateTimeProvider.UtcNow;
    }

    public void SetUpdatedAt(IDateTimeProvider dateTimeProvider)
    {
        UpdatedAt = dateTimeProvider.UtcNow;
    }
}