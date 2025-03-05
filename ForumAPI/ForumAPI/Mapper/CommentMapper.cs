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

        public static Comment FromCOmmentAPostDTO(CommentAPostDTO commentAPostDTO)
        {
            return new Comment
            {
                Content = commentAPostDTO.Content,
                PostId = commentAPostDTO.PostId,
                CreatedBy = commentAPostDTO.CreatedBy
            };
        }
    }
}
