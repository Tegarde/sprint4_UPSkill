using ForumAPI.DTOs.BadgeDTOs;

namespace ForumAPI.DTOs.GreenitorDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for a greenitor, which includes the greenitor's username, email, role, interactions, image, and associated badges.
    /// </summary>
    public class GreenitorDTO
    {
        /// <summary>
        /// Gets or sets the username of the greenitor.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the greenitor.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the role of the greenitor, such as 'Admin', 'Moderator', or 'User'.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the number of interactions the greenitor has made (e.g., posts, comments, likes).
        /// </summary>
        public int Interactions { get; set; }

        /// <summary>
        /// Gets or sets the URL or file path to the greenitor's profile image.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the list of badges associated with the greenitor.
        /// Each badge is represented by a BadgeDescriptionDTO.
        /// </summary>
        public List<BadgeDescriptionDTO> Badges { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GreenitorDTO"/> class.
        /// </summary>
        /// <param name="username">The username of the greenitor.</param>
        /// <param name="email">The email address of the greenitor.</param>
        /// <param name="interactions">The number of interactions the greenitor has made.</param>
        public GreenitorDTO(string username, string email, int interactions)
        {
            Username = username;
            Email = email;
            Interactions = interactions;
        }

        public GreenitorDTO()
        {
        }
    }
}
