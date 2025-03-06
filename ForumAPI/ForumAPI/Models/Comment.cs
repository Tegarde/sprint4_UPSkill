using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumAPI.Models
{   
    /// <summary>
    /// Represents a comment in the forum.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// The unique identifier for the comment.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier for the post.
        /// </summary>

        [ForeignKey("Post")]
        public int? PostId { get; set; }=null;

        public Post? Post { get; set; }

        /// <summary>
        /// The unique identifier for the parent comment.
        /// </summary>
        [ForeignKey("Comment")]
        public int? ParentCommentId { get; set; }=null;

        public Comment? ParentComment { get; set; }

        public int? ParentPost { get; set; }

        /// <summary>
        /// The unique identifier for the event.
        /// </summary>
        [ForeignKey("Event")]
        public int? EventId { get; set; }=null;

        public Event? Event { get; set; }

        /// <summary>
        /// The content of the comment.
        /// </summary>
        [MaxLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// The unique identifier for the user who created the comment.
        /// </summary>
        [MaxLength(100)]
        public string CreatedBy { get;set; }

        /// <summary>
        /// The date and time the comment was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The unique identifier for the users who liked the comment.
        /// </summary>
        public ICollection<CommentLike> LikedBy { get; set; } = new List<CommentLike>();

        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
