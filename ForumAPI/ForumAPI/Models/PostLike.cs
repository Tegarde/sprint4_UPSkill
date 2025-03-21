﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumAPI.Models
{   
    /// <summary>
    /// Represents a like for a post.
    /// </summary>
    public class PostLike
    {
        /// <summary>
        /// The post associated with the like.
        /// </summary>
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// The post associated with the like.
        /// </summary>
        public Post? Post { get; set; }

        /// <summary>
        /// The user associated with the like.
        /// </summary>
        [MaxLength(100)]
        public string User { get;set; }

        /// <summary>
        /// The date associated with the like.
        /// </summary>
        public DateTime LikedAt { get; set; }
    }
}
