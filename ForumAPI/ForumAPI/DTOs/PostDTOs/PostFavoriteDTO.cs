namespace ForumAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for the PostFavorite entity.
    /// </summary>
    public class PostFavoriteDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the post associated with the favorite.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who marked the post as a favorite.
        /// </summary>
        public string User { get; set; }
    }
}