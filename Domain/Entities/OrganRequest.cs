using System;
using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class OrganRequest : BaseEntity
    {
        public Guid RequesterId { get; set; }
        public AppUser Requester { get; set; } = null!;
        public RequestType Type { get; set; }
        public string Organ { get; set; } = null!;
        public DateTime? CompletedAt { get; set; }
    }
}