using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Requests.Transactions;
using Application.Responses;
using Application.Responses.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/transactions")]
    public class TransactionsController : BaseController
    {
        private readonly IHandler<ListTransactionRequest, IReadOnlyList<ListTransactionResponse>> _listTransactionHandler;
        public TransactionsController(IHandler<ListTransactionRequest, IReadOnlyList<ListTransactionResponse>> listTransactionHandler)
        {
            _listTransactionHandler = listTransactionHandler;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ListTransactionResponse>>), 200)]
        public async Task<IActionResult> ListTransactions([FromQuery] ListTransactionRequest request)
        {
            return await HandleAsync(_listTransactionHandler, request);
        }
    }
}