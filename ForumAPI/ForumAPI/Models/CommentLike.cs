using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForumAPI.Models
{   
    /// <summary>
    /// Represents a like for a comment.
    /// </summary>
    public class CommentLike
    {
       

        /// <summary>
        /// The unique identifier for the comment associated with the like.
        /// </summary>
        [ForeignKey("Comment")]
        public int CommentId { get; set; }

        /// <summary>
        /// The comment associated with the like.
        /// </summary>
        public Comment Comment { get; set; }

        /// <summary>
        /// The unique identifier for the user associated with the like.
        /// </summary>
        [MaxLength(100)]
        public string User { get; set; }

        public DateTime LikedAt { get; set; }
    }
}
