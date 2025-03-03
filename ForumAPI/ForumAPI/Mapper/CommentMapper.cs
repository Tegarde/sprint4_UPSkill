using ForumAPI.DTOs;
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
    }
}
