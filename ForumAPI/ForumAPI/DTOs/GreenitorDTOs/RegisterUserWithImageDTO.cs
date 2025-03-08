namespace ForumAPI.DTOs.GreenitorDTOs
{
    public class RegisterUserWithImageDTO
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

        public string Image { get; set; }
    }
}
