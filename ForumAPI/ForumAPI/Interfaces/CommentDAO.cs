using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    public interface CommentDAO
    {
        Comment CommentAComment(Comment comment);

        Comment CommentAnEvent(Comment comment, int eventId);
    }
}
