using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    public interface CommentDAO
    {
        Task<Comment> CommentAComment(Comment comment);

        Task<Comment> CommentAnEvent(Comment comment);

        Task<Comment> CommentAPost(Comment comment);

        Task<GreenitorStatisticsDTO> GetCommentStatisticsByUsername(string username);

        Task<CommentLike> UnLikeComment(CommentLike commentLike);

        Task<CommentLike> LikeComment(CommentLike commentLike);

        Task<int> GetNumberOfLikesFromCommentId(int commentId);

        Task<int> GetCommentInteractionsByUser(int commentId, string username);
    }
}
