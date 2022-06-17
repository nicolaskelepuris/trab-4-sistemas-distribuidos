using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IBlockchain
    {
        Task<Transaction> CreateTransaction(OrganRequest first, OrganRequest second);
    }

    public class BlockchainSimulator : IBlockchain
    {
        public Task<Transaction> CreateTransaction(OrganRequest first, OrganRequest second)
        {
            Validate(first, second);

            var (from, to) = GetFromTo(first, second);
            var transaction = new Transaction
            {
                From = from,
                To = to,
                Organ = first.Organ
            };
            return Task.FromResult(transaction);
        }

        private void Validate(OrganRequest first, OrganRequest second)
        {
            if (string.IsNullOrWhiteSpace(first?.Organ) || string.IsNullOrWhiteSpace(second?.Organ) || first.Organ != second.Organ) throw new Exception("Invalid organ");
            if (first.Requester == null || second.Requester == null) throw new Exception("Invalid requester");
            if (first.Type == second.Type) throw new Exception("Organ request types do not match");
        }

        private (AppUser from, AppUser to) GetFromTo(OrganRequest first, OrganRequest second)
        {
            if (first.Type == RequestType.Give)
            {
                return (first.Requester, second.Requester);
            }

            return (second.Requester, first.Requester);
        }
    }
}