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
        /// The unique identifier for the post dislike.
        /// </summary>
        [Key]
        public int Id { get; set; }

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
        public string User { get; set; }
    }
}
