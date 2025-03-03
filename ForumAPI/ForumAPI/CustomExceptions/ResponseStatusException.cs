using ForumAPI.DTOs;
using System.Net;

namespace ForumAPI
{
    public class ResponseStatusException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ResponseMessage ResponseMessage { get; set; }

        public ResponseStatusException(HttpStatusCode statusCode, ResponseMessage responseMessage)
        {
            StatusCode = statusCode;
            ResponseMessage = responseMessage;
        }
    }
}
