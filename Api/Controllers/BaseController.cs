using Application.Exceptions;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected async Task<IActionResult> Handle<TRequest, TResponse>(IHandler<TRequest, TResponse> handler, TRequest request)
        {
            TResponse? data;
            try
            {
                ArgumentNullException.ThrowIfNull(handler);
                ArgumentNullException.ThrowIfNull(request);
                data = await handler.Handle(request);
            }
            catch (ApiException ex)
            {
                return StatusCode((int)ex.StatusCode, new ApiResponse<string>
                {
                    Success = false,
                    Error = new Error(ex.Message)
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>
                {
                    Success = false,
                    Error = new Error(ex.Message)
                });
            }

            return Ok(new ApiResponse<TResponse>()
            {
                Success = true,
                Data = data
            });
        }
    }
}
