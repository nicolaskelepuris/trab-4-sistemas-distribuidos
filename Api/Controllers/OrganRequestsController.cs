using System.Threading.Tasks;
using Application.Interfaces;
using Application.Requests.OrganRequests;
using Application.Responses;
using Application.Responses.OrganRequests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/organ-requests")]
    public class OrganRequestsController : BaseController
    {
        private readonly IHandler<CreateOrganRequestRequest, CreateOrganRequestResponse> _createOrganRequestHandler;
        public OrganRequestsController(IHandler<CreateOrganRequestRequest, CreateOrganRequestResponse> createOrganRequestHandler)
        {
            _createOrganRequestHandler = createOrganRequestHandler;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CreateOrganRequestResponse>), 200)]
        public async Task<IActionResult> CreateOrganRequest([FromBody] CreateOrganRequestRequest request)
        {
            return await HandleAsync(_createOrganRequestHandler, request);
        }
    }
}