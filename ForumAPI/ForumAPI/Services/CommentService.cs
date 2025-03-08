using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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

        public async Task<Comment> CommentAComment(Comment comment)
        {
            //Check if parent comment exists
            var parentComment = context.Comments.FirstOrDefault(c => c.Id == comment.ParentCommentId);

            //If parent comment does not exist, throw exception
            if (parentComment == null)
            {
                throw new NotFoundException("Parent comment not found.");
            }

            GreenitorDTO user = await greenitorDAO.GetUserByUsername(comment.CreatedBy);

            // Set parent comment
            comment.ParentComment = parentComment;

            if (parentComment.ParentPostId != null)
            {
                comment.ParentPostId = parentComment.ParentPostId;
            }

            // Set created at
            comment.CreatedAt = DateTime.UtcNow;



            context.Comments.Add(comment);

            context.SaveChanges();

            await greenitorDAO.IncrementUserInteractions(comment.CreatedBy);

            return comment;
        }

        public async Task<Comment> CommentAnEvent(Comment comment)
        {
            var ev = context.Events.FirstOrDefault(e => e.Id == comment.EventId);
            if (ev == null)
            {
                throw new NotFoundException("Event not found");
            }

            //Get user from greenitorDAO
            GreenitorDTO user = await greenitorDAO.GetUserByUsername(comment.CreatedBy);
            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }

            comment.Event = ev;
            comment.CreatedAt = DateTime.UtcNow;
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            await greenitorDAO.IncrementUserInteractions(comment.CreatedBy);
            return comment;
        }

        public async Task<Comment> CommentAPost(Comment comment)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == comment.PostId);
            if (post == null)
            { throw new NotFoundException("Post not found"); }

            GreenitorDTO user = await greenitorDAO.GetUserByUsername(comment.CreatedBy);
            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }
            comment.Post = post;
            comment.ParentPostId = post.Id;

            comment.CreatedAt = DateTime.UtcNow;

            context.Comments.Add(comment);

            await context.SaveChangesAsync();

            await greenitorDAO.IncrementUserInteractions(comment.CreatedBy);

            return comment;
        }

        public async Task<GreenitorStatisticsDTO> GetCommentStatisticsByUsername(string username)
        {

            var comments = await context.Comments.Where(c => c.CreatedBy == username).CountAsync();

            var likes = await context.CommentLikes.Where(cl => cl.User == username).CountAsync();

            return new GreenitorStatisticsDTO
            {
                Comments = comments,
                LikesInComments = likes
            };
        }

        public async Task<CommentLike> LikeComment(CommentLike commentLike)
        {
            var comment = context.Comments.FirstOrDefault(c => c.Id == commentLike.CommentId);
            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }

            GreenitorDTO user = await greenitorDAO.GetUserByUsername(commentLike.User);
            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }

            CommentLike? like = context.CommentLikes.FirstOrDefault(cl => cl.CommentId == commentLike.CommentId && cl.User == commentLike.User);
            if (like != null)
            {
                throw new ArgumentException("User already liked this comment");
            }

            commentLike.Comment = comment;
            context.CommentLikes.Add(commentLike);
            await greenitorDAO.IncrementUserInteractions(commentLike.User);
            await context.SaveChangesAsync();
            return commentLike;
        }

        public async Task<CommentLike> UnLikeComment(CommentLike commentLike)
        {
            var comment = context.Comments.FirstOrDefault(c => c.Id == commentLike.CommentId);
            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }

            GreenitorDTO user = await greenitorDAO.GetUserByUsername(commentLike.User);
            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }

            CommentLike? like = context.CommentLikes.FirstOrDefault(cl => cl.CommentId == commentLike.CommentId && cl.User == commentLike.User);
            if (like == null)
            {
                throw new ArgumentException("User has not liked this comment");
            }

            context.CommentLikes.Remove(like);
            await context.SaveChangesAsync();
            await greenitorDAO.DecrementUserInteractions(commentLike.User);
            return like;

        }

        public async Task<int> GetNumberOfLikesFromCommentId(int commentId)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }
            return await context.CommentLikes.Where(cl => cl.CommentId == commentId).CountAsync();

        }

        public async Task<int> GetCommentInteractionsByUser(int commentId, string username)
        {
            var user = await greenitorDAO.GetUserByUsername(username);
            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }

            var commentExists = await context.Comments.AnyAsync(c => c.Id == commentId);
            if (!commentExists)
            {
                throw new NotFoundException("Comment not found");
            }

            if (await context.CommentLikes.AnyAsync(cl => cl.CommentId == commentId && cl.User == username))
            {
                return 1; // Retorna 1 se o utilizador deu Like
            }

            return 0; // Retorna 0 se não houver interação
        }

        public async Task<List<Comment>> GetCommentsByCommentId(int commentId)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }
            return await context.Comments.Where(c => c.ParentCommentId == commentId).ToListAsync();
        }




    }
}

