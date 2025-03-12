namespace ForumAPI.DTOs.PostDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for PostNotification, representing a post's notification details.
    /// </summary>
    public class PostNotificationDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the post.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the number of interactions (likes, comments, etc.) the post has received.
        /// </summary>
        public int Interactions { get; set; }
    }
}