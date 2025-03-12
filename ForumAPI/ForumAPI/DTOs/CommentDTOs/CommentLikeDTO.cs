namespace ForumAPI.DTOs.CommentDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing a like on a comment.
    /// </summary>
    public class CommentLikeDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment that is being liked.
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who liked the comment.
        /// </summary>
        public string User { get; set; }
    }
}