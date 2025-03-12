namespace ForumAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for the PostDislike entity.
    /// </summary>
    public class PostDislikeDTO
    {
        /// <summary>
        /// Gets or sets the post ID associated with the dislike.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who disliked the post.
        /// </summary>
        public string User { get; set; }
    }
}