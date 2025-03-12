using ForumAPI.DTOs.BadgeDTOs;

namespace ForumAPI.DTOs.GreenitorDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for a greenitor excluding their role.
    /// </summary>
    public class GreenitorWithoutRoleDTO
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
        /// Gets or sets the number of interactions the greenitor has made (e.g., posts, comments, likes).
        /// This property is nullable, meaning it can be absent or not assigned.
        /// </summary>
        public int? Interactions { get; set; }

        /// <summary>
        /// Gets or sets the URL or file path of the greenitor's profile image.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the list of badges associated with the greenitor.
        /// Each badge is represented by a BadgeDescriptionDTO.
        /// </summary>
        public List<BadgeDescriptionDTO> Badges { get; set; }
    }
}