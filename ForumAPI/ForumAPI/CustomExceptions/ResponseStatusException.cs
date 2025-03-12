using ForumAPI.DTOs;
using System.Net;

namespace ForumAPI
{
    /// <summary>
    /// Custom exception that represents an error response with a specific HTTP status code and a custom message.
    /// This exception is useful for handling specific HTTP error responses with additional message details.
    /// </summary>
    public class ResponseStatusException : Exception
    {
        /// <summary>
        /// Gets or sets the HTTP status code associated with the exception.
        /// The status code helps determine the nature of the error (e.g., 404 for Not Found, 400 for Bad Request).
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the custom response message associated with the exception.
        /// This message provides additional details about the error, typically sent in the response body.
        /// </summary>
        public ResponseMessage ResponseMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseStatusException"/> class with a specified HTTP status code and response message.
        /// This constructor is used when you need to specify both the status code and the detailed error message.
        /// </summary>
        /// <param name="statusCode">The HTTP status code that represents the type of error (e.g., 404, 500).</param>
        /// <param name="responseMessage">A custom response message that explains the error in more detail.</param>
        public ResponseStatusException(HttpStatusCode statusCode, ResponseMessage responseMessage)
        {
            StatusCode = statusCode;  // Sets the HTTP status code
            ResponseMessage = responseMessage;  // Sets the custom response message
        }
    }
}