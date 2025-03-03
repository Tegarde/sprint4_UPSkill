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
        /// The unique identifier for the post favorite.
        /// </summary>
        [Key]
        public int Id { get; set; }

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
        public string User { get; set; }

    }
}
