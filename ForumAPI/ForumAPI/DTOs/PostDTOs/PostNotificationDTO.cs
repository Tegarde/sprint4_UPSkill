namespace ForumAPI.DTOs.PostDTOs
{
    /// <summary>
    /// Data Transfer Object for PostNotification
    /// </summary>
    public class PostNotificationDTO
    {
        /// <summary>
        /// Id of the Post
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Title of the Post
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Number of interactions
        /// </summary>
        public int Interactions { get; set; }
    }
}
