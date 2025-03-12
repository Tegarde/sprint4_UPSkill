namespace ForumAPI.DTOs.BadgeDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a badge with description and interaction count.
    /// </summary>
    public class BadgeDTO
    {
        /// <summary>
        /// Gets or sets the description of the badge.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the number of interactions required to earn the badge.
        /// </summary>
        public int Interactions { get; set; }
    }
}