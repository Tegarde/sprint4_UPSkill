using System.ComponentModel.DataAnnotations;

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
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// The content of the post.
        /// </summary>
        [MaxLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// The unique identifier for the user who created the post.
        /// </summary>
        [MaxLength(100)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// The date and time the post was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// The status of the post.
        /// </summary>
        [Required]
        public bool Status { get; set; }

        /// <summary>
        /// The category of the post.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Category {get;set;}

        /// <summary>
        /// The number of interactions with the post since the user's last visit.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int Interactions { get; set; }

        /// <summary>
        /// The image of the post.
        /// </summary>
        public string? Image { get;set; }

        /// <summary>
        /// The unique identifiers for the comments on the post.
        /// </summary>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// The unique identifiers for the users who liked the post.
        /// </summary>
        public ICollection<PostLike> LikedBy { get; set; } = new List<PostLike>();

        /// <summary>
        /// The unique identifiers for the users who disliked the post.
        /// </summary>
        public ICollection<PostDislike> DislikedBy { get; set; } = new List<PostDislike>();

        /// <summary>
        /// The unique identifiers for the users who favorited the post.
        /// </summary>
        public ICollection<PostFavorite> FavoritedBy { get; set; } = new List<PostFavorite>();
    }
}
