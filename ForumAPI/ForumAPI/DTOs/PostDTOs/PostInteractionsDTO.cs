namespace ForumAPI.DTOs.PostDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for the interactions (likes and dislikes) of a post.
    /// </summary>
    public class PostInteractionsDTO
    {
        /// <summary>
        /// Gets or sets the number of likes associated with the post.
        /// </summary>
        public int Likes { get; set; }

        /// <summary>
        /// Gets or sets the number of dislikes associated with the post.
        /// </summary>
        public int Dislikes { get; set; }
    }
}