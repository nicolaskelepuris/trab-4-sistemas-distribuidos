using Application.Interfaces;
using Application.Requests.Transactions;
using Application.Responses.Transactions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Handlers.Transactions
{
    public class ListTransactionHandler : IHandler<ListTransactionRequest, IReadOnlyList<ListTransactionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ListTransactionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<ListTransactionResponse>> HandleAsync(ListTransactionRequest request)
        {
            var transactions = await GetTransactionsAsync();
            return transactions.Select(ToResponse).ToList();
        }

        private async Task<IReadOnlyList<Transaction>> GetTransactionsAsync()
        {
            var specification = new Specification<Transaction>();
            specification.AddInclude(x => x.From);
            specification.AddInclude(x => x.To);
            return await _unitOfWork.Repository<Transaction>().ListAsyncWithSpec(specification) ?? new List<Transaction>();
        }

        private ListTransactionResponse ToResponse(Transaction transaction)
        {
            return new ListTransactionResponse
            {
                From = ToUserResponse(transaction.From),
                To = ToUserResponse(transaction.To),
                Organ = transaction.Organ,
                Date = transaction.CreatedAt
            };
        }

        private ListTransactionUserResponse ToUserResponse(AppUser user)
        {
            return new ListTransactionUserResponse
            {
                Name = user.Name,
                Id = user.Id
            };
        }
    }
}
