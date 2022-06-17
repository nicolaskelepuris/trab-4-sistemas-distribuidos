using System.Collections.Generic;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = null!;
        public ICollection<OrganRequest> OrganRequests { get; set; } = null!;
    }
}
