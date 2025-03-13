using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForumAPI.Models
{   
    /// <summary>
    /// Represents a dislike for a post.
    /// </summary>
    public class PostDislike
    {
        /// <summary>
        /// The post associated with the dislike.
        /// </summary>
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// The post associated with the dislike.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// The user associated with the dislike.
        /// </summary>
        [MaxLength(100)]
        public string User { get; set; }

        /// <summary>
        /// The date associated with the dislike.
        /// </summary>
        public DateTime DislikedAt { get; set; }
    }
}
