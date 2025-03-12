namespace ForumAPI.DTOs.BadgeDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for providing badge descriptions.
    /// </summary>
    public class BadgeDescriptionDTO
    {
        /// <summary>
        /// Gets or sets the description of the badge.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the badge (optional).
        /// </summary>
        public string? Image { get; set; }
    }
}