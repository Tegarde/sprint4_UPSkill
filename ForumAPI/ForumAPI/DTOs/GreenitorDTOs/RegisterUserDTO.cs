namespace ForumAPI.DTOs.GreenitorDTOs
{
    /// <summary>
    /// Data transfer object for user registration
    /// </summary>
    public class RegisterUserDTO
    {
        
        /// <summary>
        /// Username to register
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email to register
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password to register
        /// </summary>
        public string Password { get; set; }
    }
}
