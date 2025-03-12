namespace ForumAPI.DTOs.PostDTOs
{
    /// <summary>
    /// Data transfer object (DTO) for creating a new post.
    /// </summary>
    public class CreatePostDTO
    {
        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who created the post.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the category under which the post is classified (e.g., "Discussion", "News").
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the image associated with the post.
        /// This property is optional and can be null if no image is provided.
        /// </summary>
        public IFormFile? Image { get; set; }
    }
}