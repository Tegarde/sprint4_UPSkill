using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    public interface CommentDAO
    {
       Task<Comment> CommentAComment(Comment comment);

       Task<Comment> CommentAnEvent(Comment comment);

        Task<Comment> CommentAPost(Comment comment);
    }
}
