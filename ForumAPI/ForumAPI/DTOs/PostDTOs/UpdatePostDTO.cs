namespace ForumAPI.DTOs.PostDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for updating an existing post.
    /// </summary>
    public class UpdatePostDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the post to be updated.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the new content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the new title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the username of the person who created or is updating the post.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}