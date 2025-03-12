namespace ForumAPI.DTOs
{
    /// <summary>
    /// Represents a response message for the API, typically used to convey success or error messages.
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// Gets or sets the content of the response message.
        /// This message provides information about the status of an API request.
        /// </summary>
        public string Message { get; set; }
    }
}