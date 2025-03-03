using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForumAPI.Models
{   
    /// <summary>
    /// Represents a favorite for a post.
    /// </summary>
    public class PostFavorite
    {
        /// <summary>
        /// The post associated with the favorite.
        /// </summary>
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// The post associated with the favorite.
        /// </summary>
        public Post Post { get; set; }

        /// <summary>
        /// The user associated with the favorite.
        /// </summary>
        [MaxLength(100)]
        public string User { get; set; }

    }
}
