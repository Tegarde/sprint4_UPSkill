namespace ForumAPI.DTOs.GreenitorDTOs
{
    /// <summary>
    /// Data transfer object for user registration with an optional image.
    /// </summary>
    public class RegisterUserWithImageDTO
    {
        /// <summary>
        /// Gets or sets the username for registering the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address for registering the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for registering the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the profile image URL for the user during registration.
        /// This property is optional, and it can be null if no image is provided.
        /// </summary>
        public string? Image { get; set; }
    }
}