namespace ForumAPI.DTOs.GreenitorDTOs
{
    /// <summary>
    /// Data transfer object for user registration.
    /// </summary>
    public class RegisterUserDTO
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
        /// Gets or sets the profile image for the user during registration.
        /// This property is optional and may not be provided.
        /// </summary>
        public IFormFile? Image { get; set; }
    }
}