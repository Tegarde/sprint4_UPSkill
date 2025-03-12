namespace ForumAPI.DTOs.GreenitorDTOs
{
    /// <summary>
    /// Data transfer object (DTO) representing a token containing user details like username and role.
    /// </summary>
    public class TokenDTO
    {
        /// <summary>
        /// Gets or sets the username of the user associated with the token.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the role of the user associated with the token (e.g., Admin, User, Moderator).
        /// </summary>
        public string Role { get; set; }
    }
}