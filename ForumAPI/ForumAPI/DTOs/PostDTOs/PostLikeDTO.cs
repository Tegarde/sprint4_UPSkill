namespace ForumAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for the PostLike entity.
    /// </summary>
    public class PostLikeDTO
    {
        /// <summary>
        /// The post ID associated with the like.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// The user associated with the like.
        /// </summary>
        public string User { get; set; }
    }
}
