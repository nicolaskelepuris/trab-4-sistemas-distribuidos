using System;

namespace Application.Responses.Transactions
{
    public class ListTransactionResponse
    {
        public ListTransactionUserResponse From { get; set; } = null!;
        public ListTransactionUserResponse To { get; set; } = null!;
        public string Organ { get; set; } = null!;
        public DateTime Date { get; set; }
    }

    public class ListTransactionUserResponse
    {
        public string? Name { get; set; }
        public Guid Id { get; set; }
    }
}
