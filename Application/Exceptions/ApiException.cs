using System;
using System.Net;

namespace Application.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string? message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; private set; }

        public static ApiException BadRequest(string invalidField)
        {
            return new ApiException($"Invalid {invalidField}", HttpStatusCode.BadRequest);
        }
    }
}
