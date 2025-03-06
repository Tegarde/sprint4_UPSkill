using ForumAPI.DTOs.CommentDTOs;
using ForumAPI.Models;

namespace ForumAPI.Mapper
{
    public class CommentMapper
    {
        public static Comment FromCommentACommentDTO(CommentACommentDTO commentACommentDTO)
        {
            return new Comment
            {
                ParentCommentId = commentACommentDTO.ParentCommentId,
                Content = commentACommentDTO.Content,
                CreatedBy = commentACommentDTO.CreatedBy,
            };
        }

        public static Comment FromCommentAnEventDTO(CommentAnEventDTO commentAnEventDTO)
        {
            return new Comment
            {   
                EventId = commentAnEventDTO.EventId,
                Content = commentAnEventDTO.Content,
                CreatedBy = commentAnEventDTO.CreatedBy,
            };
        }

        public static Comment FromCommentAPostDTO(CommentAPostDTO commentAPostDTO)
        {
            return new Comment
            {
                Content = commentAPostDTO.Content,
                PostId = commentAPostDTO.PostId,
                CreatedBy = commentAPostDTO.CreatedBy
            };
        }

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
