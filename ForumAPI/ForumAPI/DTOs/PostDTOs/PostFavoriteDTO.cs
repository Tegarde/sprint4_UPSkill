namespace ForumAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object for the PostFavorite entity.
    /// </summary>
    public class PostFavoriteDTO
    {
        /// <summary>
        /// The post ID associated with the favorite.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// The user associated with the favorite.
        /// </summary>
        public string User { get; set; }
    }
}
