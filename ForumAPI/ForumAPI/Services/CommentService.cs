using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.Interfaces;
using ForumAPI.Models;

namespace ForumAPI.Services
{
    public class CommentService : CommentDAO
    {

        private readonly DataContext context;

        private readonly GreenitorDAO greenitorDAO;

        public CommentService(DataContext context, GreenitorDAO greenitorDAO)
        {
            this.context = context;
            this.greenitorDAO = greenitorDAO;
        }

        public Comment CommentAComment(Comment comment)
        {   
            //Check if parent comment exists
            var parentComment = context.Comments.FirstOrDefault(c => c.Id == comment.ParentCommentId);

            //If parent comment does not exist, throw exception
            if (parentComment == null)
            {
                throw new NotFoundException("Parent comment not found.");
            }

            // Set parent comment
            comment.ParentComment = parentComment;

            // Set created at
            comment.CreatedAt = DateTime.UtcNow;

            // TODO Check if user exists

            context.Comments.Add(comment);
            context.SaveChanges();

            return comment;
        }
    }
}
