namespace ForumAPI.DTOs.CommentDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for replying to a comment.
    /// </summary>
    public class CommentACommentDTO
    {
        /// <summary>
        /// Gets or sets the ID of the parent comment being replied to.
        /// </summary>
        public int ParentCommentId { get; set; }

        /// <summary>
        /// Gets or sets the content of the reply.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who created the reply.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}