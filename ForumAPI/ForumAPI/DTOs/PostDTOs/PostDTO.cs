using ForumAPI.DTOs.CommentDTOs;

namespace ForumAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for the Post entity.
    /// </summary>
    public class PostDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the post.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the username of the person who created the post.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the post was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the count of users who liked the post.
        /// </summary>
        public int LikedBy { get; set; }

        /// <summary>
        /// Gets or sets the count of users who disliked the post.
        /// </summary>
        public int DislikedBy { get; set; }

        /// <summary>
        /// Gets or sets the status of the post (e.g., active, inactive).
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Gets or sets the category of the post (e.g., "Discussion", "News").
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the number of interactions with the post, such as likes, comments, or shares.
        /// </summary>
        public int Interactions { get; set; }

        /// <summary>
        /// Gets or sets the URL or file path of the image associated with the post.
        /// This property is optional and can be null if no image is provided.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the list of comments associated with the post.
        /// Each comment is represented by a <see cref="CommentFromPostDTO"/>.
        /// </summary>
        public List<CommentFromPostDTO> Comments { get; set; }
    }
}