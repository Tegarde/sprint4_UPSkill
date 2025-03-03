namespace ForumAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for the Post entity.
    /// </summary>
    public class PostDTO
    {
        /// <summary>
        /// The title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The username of the person who created the post.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// The date and time the post was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The status of the post.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// The category of the post.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The number of interactions with the post.
        /// </summary>
        public int Interactions { get; set; }
    }
}
