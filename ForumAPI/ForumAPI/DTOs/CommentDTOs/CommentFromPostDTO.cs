namespace ForumAPI.DTOs.CommentDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a comment on a post.
    /// </summary>
    public class CommentFromPostDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the post that this comment is associated with.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the content of the comment.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who created the comment.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the comment was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the number of users who liked this comment.
        /// </summary>
        public int LikedBy { get; set; }

        /// <summary>
        /// Gets or sets the number of replies this comment has received.
        /// </summary>
        public int CommentsCounter { get; set; }
    }
}