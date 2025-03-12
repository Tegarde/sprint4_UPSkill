namespace ForumAPI.DTOs.CommentDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for commenting on an event.
    /// </summary>
    public class CommentAnEventDTO
    {
        /// <summary>
        /// Gets or sets the ID of the event being commented on.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who created the comment.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}