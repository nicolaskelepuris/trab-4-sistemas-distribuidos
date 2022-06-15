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
    }
}
