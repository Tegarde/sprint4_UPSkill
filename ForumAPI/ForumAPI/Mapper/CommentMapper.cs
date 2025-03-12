using ForumAPI.DTOs.CommentDTOs;
using ForumAPI.Models;

namespace ForumAPI.Mapper
{
    /// <summary>
    /// A helper class to map between various DTOs and the Comment model.
    /// </summary>
    public class CommentMapper
    {
        /// <summary>
        /// Maps a CommentACommentDTO to a Comment model.
        /// </summary>
        /// <param name="commentACommentDTO">The CommentACommentDTO to be mapped.</param>
        /// <returns>A Comment model with data from the provided CommentACommentDTO.</returns>
        public static Comment FromCommentACommentDTO(CommentACommentDTO commentACommentDTO)
        {
            return new Comment
            {
                ParentCommentId = commentACommentDTO.ParentCommentId,
                Content = commentACommentDTO.Content,
                CreatedBy = commentACommentDTO.CreatedBy,
            };
        }

        /// <summary>
        /// Maps a CommentAnEventDTO to a Comment model.
        /// </summary>
        /// <param name="commentAnEventDTO">The CommentAnEventDTO to be mapped.</param>
        /// <returns>A Comment model with data from the provided CommentAnEventDTO.</returns>
        public static Comment FromCommentAnEventDTO(CommentAnEventDTO commentAnEventDTO)
        {
            return new Comment
            {
                EventId = commentAnEventDTO.EventId,
                Content = commentAnEventDTO.Content,
                CreatedBy = commentAnEventDTO.CreatedBy,
            };
        }

        /// <summary>
        /// Maps a CommentAPostDTO to a Comment model.
        /// </summary>
        /// <param name="commentAPostDTO">The CommentAPostDTO to be mapped.</param>
        /// <returns>A Comment model with data from the provided CommentAPostDTO.</returns>
        public static Comment FromCommentAPostDTO(CommentAPostDTO commentAPostDTO)
        {
            return new Comment
            {
                Content = commentAPostDTO.Content,
                PostId = commentAPostDTO.PostId,
                CreatedBy = commentAPostDTO.CreatedBy
            };
        }

        /// <summary>
        /// Maps a Comment model to a CommentFromPostDTO.
        /// </summary>
        /// <param name="comment">The Comment model to be mapped.</param>
        /// <returns>A CommentFromPostDTO with data from the provided Comment model.</returns>
        public static CommentFromPostDTO ToCommentFromPostDTO(Comment comment)
        {
            return new CommentFromPostDTO
            {
                Id = comment.Id,
                PostId = (int)comment.PostId,
                Content = comment.Content,
                CreatedBy = comment.CreatedBy,
                CreatedAt = comment.CreatedAt,
                LikedBy = comment.LikedBy.Count,
                CommentsCounter = comment.Replies.Count
            };
        }

        /// <summary>
        /// Maps a Comment model to a CommentFromCommentDTO.
        /// </summary>
        /// <param name="comment">The Comment model to be mapped.</param>
        /// <returns>A CommentFromCommentDTO with data from the provided Comment model.</returns>
        public static CommentFromCommentDTO ToCommentFromCommentDTO(Comment comment)
        {
            return new CommentFromCommentDTO
            {
                Id = comment.Id,
                ParentCommentId = (int)comment.ParentCommentId,
                Content = comment.Content,
                CreatedBy = comment.CreatedBy,
                CreatedAt = comment.CreatedAt,
                LikedBy = comment.LikedBy.Count,
                CommentsCounter = comment.Replies.Count
            };
        }

        /// <summary>
        /// Maps a Comment model to a CommentFromEventDTO.
        /// </summary>
        /// <param name="comment">The Comment model to be mapped.</param>
        /// <returns>A CommentFromEventDTO with data from the provided Comment model.</returns>
        public static CommentFromEventDTO ToCommentFromEventDTO(Comment comment)
        {
            return new CommentFromEventDTO
            {
                Id = comment.Id,
                EventId = (int)comment.EventId,
                Content = comment.Content,
                CreatedBy = comment.CreatedBy,
                CreatedAt = comment.CreatedAt,
                LikedBy = comment.LikedBy.Count,
                CommentsCounter = comment.Replies.Count
            };
        }
    }
}