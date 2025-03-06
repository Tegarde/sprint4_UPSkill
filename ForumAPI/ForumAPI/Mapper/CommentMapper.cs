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

        public static Comment FromCommentWithCommentsDTO(CommentWithCommentsDTO commentWithCommentsDTO)
        {
            return new Comment
            {
                PostId = commentWithCommentsDTO.PostId,
                ParentCommentId = commentWithCommentsDTO.ParentCommentId,
                EventId = commentWithCommentsDTO.EventId,
                Content = commentWithCommentsDTO.Content,
                CreatedBy = commentWithCommentsDTO.CreatedBy,
                CreatedAt = commentWithCommentsDTO.CreatedAt,
                LikedBy = commentWithCommentsDTO.LikedBy,
                Comments = 
            };
        }

        public static Comment FromCommentWithoutCommentsDTO(CommentWithoutCommentsDTO commentWithoutCommentsDTO)
        {
            return new Comment
            {
                PostId = commentWithoutCommentsDTO.PostId,
                ParentCommentId = commentWithoutCommentsDTO.ParentCommentId,
                EventId = commentWithoutCommentsDTO.EventId,
                Content = commentWithoutCommentsDTO.Content,
                CreatedBy = commentWithoutCommentsDTO.CreatedBy,
                CreatedAt = commentWithoutCommentsDTO.CreatedAt,
                LikedBy = commentWithoutCommentsDTO.LikedBy,
                Comments =
            };
        }
    }
}
