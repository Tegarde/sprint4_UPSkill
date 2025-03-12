namespace ForumAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for the PostLike entity.
    /// </summary>
    public class PostLikeDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the post associated with the like.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who liked the post.
        /// </summary>
        public string User { get; set; }
    }
}