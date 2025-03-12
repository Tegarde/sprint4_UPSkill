namespace ForumAPI.CustomExceptions
{
    /// <summary>
    /// Custom exception that is thrown when a requested user is not found in the system.
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UserNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a default error message.
        /// </summary>
        public UserNotFoundException() : base()
        {
            // Default constructor that can be used when no specific message is provided.
        }
    }
}