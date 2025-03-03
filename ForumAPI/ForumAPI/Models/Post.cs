namespace ForumAPI.Models
{   
    /// <summary>
    /// Represents a post in the forum.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// The unique identifier for the post.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The title of the post.
        /// </summary>
        public string Title { get; set; }
    
        /// <summary>
        /// The content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The unique identifier for the user who created the post.
        /// </summary>
        [ForeignKey("User")]
        public string CreatedBy { get; set; }   

        /// <summary>
        /// The date and time the post was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The status of the post.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// The unique identifiers for the comments on the post.
        /// </summary>
        public ICollection<int> Comments { get; set; } = new List<int>();

        /// <summary>
        /// The unique identifiers for the users who liked the post.
        /// </summary>
        public ICollection<string> LikedBy { get; set; } = new List<string>();

        /// <summary>
        /// The unique identifiers for the users who disliked the post.
        /// </summary>
        public ICollection<string> DislikedBy { get; set; } = new List<string>();

        /// <summary>
        /// The unique identifiers for the users who favorited the post.
        /// </summary>
        public ICollection<string> FavoritedBy { get; set; } = new List<string>();
 
    }
}
