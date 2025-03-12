namespace ForumAPI.DTOs.GreenitorDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for the statistics of a greenitor's activity.
    /// </summary>
    public class GreenitorStatisticsDTO
    {
        /// <summary>
        /// Gets or sets the total number of likes the greenitor has received on their posts.
        /// </summary>
        public int LikesInPosts { get; set; }

        /// <summary>
        /// Gets or sets the total number of dislikes the greenitor has received on their posts.
        /// </summary>
        public int DislikesInPosts { get; set; }

        /// <summary>
        /// Gets or sets the total number of likes the greenitor has received on their comments.
        /// </summary>
        public int LikesInComments { get; set; }

        /// <summary>
        /// Gets or sets the total number of event attendances by the greenitor.
        /// </summary>
        public int EventAttendances { get; set; }

        /// <summary>
        /// Gets or sets the total number of posts created by the greenitor.
        /// </summary>
        public int Posts { get; set; }

        /// <summary>
        /// Gets or sets the total number of comments made by the greenitor.
        /// </summary>
        public int Comments { get; set; }

        /// <summary>
        /// Gets or sets the total number of posts that the greenitor has marked as favorites.
        /// </summary>
        public int FavoritePosts { get; set; }
    }
}