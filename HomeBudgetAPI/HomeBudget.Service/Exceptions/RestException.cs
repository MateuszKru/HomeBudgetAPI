using System.Net;

namespace HomeBudget.Service.Exceptions
{
    public class RestException : Exception
    {
        public int StatusCode { get; private set; }

        public RestException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = (int)statusCode;
        }
    }
}