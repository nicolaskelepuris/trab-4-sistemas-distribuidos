using System;
using Domain.Entities.Base;

namespace Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid FromId { get; set; }
    public AppUser From { get; set; } = null!;
    public Guid ToId { get; set; }
    public AppUser To { get; set; } = null!;
    public string Organ { get; set; } = null!;
}
