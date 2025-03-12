namespace ForumAPI.CustomExceptions
{
    /// <summary>
    /// Custom exception that is thrown when a requested resource is not found.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a default error message.
        /// </summary>
        public NotFoundException() : base()
        {
            // Default constructor that can be used when no specific message is provided.
        }
    }
}