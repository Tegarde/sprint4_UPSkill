namespace ForumAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for the PostDislike entity.
    /// </summary>
    public class PostDislikeDTO
    {
        /// <summary>
        /// The post ID associated with the dislike.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// The user associated with the dislike.
        /// </summary>
        public string User { get; set; }
    }
}
