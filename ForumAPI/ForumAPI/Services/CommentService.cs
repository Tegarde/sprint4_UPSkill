using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumAPI.Services
{
    /// <summary>
    /// Service responsible for managing comments in the forum.
    /// </summary>
    public class CommentService : CommentDAO
    {
        private readonly DataContext context;
        private readonly GreenitorDAO greenitorDAO;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="greenitorDAO">DAO for user interactions.</param>
        public CommentService(DataContext context, GreenitorDAO greenitorDAO)
        {
            this.context = context;
            this.greenitorDAO = greenitorDAO;
        }

        /// <summary>
        /// Adds a comment as a reply to another comment.
        /// </summary>
        /// <param name="comment">The comment to be added.</param>
        /// <returns>The created comment.</returns>
        /// <exception cref="NotFoundException">Thrown if the parent comment is not found.</exception>
        public async Task<Comment> CommentAComment(Comment comment)
        {
            var parentComment = context.Comments.FirstOrDefault(c => c.Id == comment.ParentCommentId);

            if (parentComment == null)
            {
                throw new NotFoundException("Parent comment not found.");
            }

            await greenitorDAO.GetUserByUsername(comment.CreatedBy);
            comment.ParentComment = parentComment;

            if (parentComment.ParentPostId != null)
            {
                var post = context.Posts.FirstOrDefault(p => p.Id == parentComment.ParentPostId);
                if (post != null)
                {
                    post.Interactions++;
                }
                comment.ParentPostId = parentComment.ParentPostId;
            }

            comment.CreatedAt = DateTime.UtcNow;
            context.Comments.Add(comment);
            context.SaveChanges();
            await greenitorDAO.IncrementUserInteractions(comment.CreatedBy);

            return comment;
        }

        /// <summary>
        /// Adds a comment to an event.
        /// </summary>
        /// <param name="comment">The comment to be added.</param>
        /// <returns>The created comment.</returns>
        /// <exception cref="NotFoundException">Thrown if the event is not found.</exception>
        public async Task<Comment> CommentAnEvent(Comment comment)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var ev = context.Events.FirstOrDefault(e => e.Id == comment.EventId);
                if (ev == null)
                {
                    throw new NotFoundException("Event not found");
                }

                GreenitorDTO user = await greenitorDAO.GetUserByUsername(comment.CreatedBy);
                comment.Event = ev;
                comment.CreatedAt = DateTime.UtcNow;
                context.Comments.Add(comment);
                await context.SaveChangesAsync();
                await greenitorDAO.IncrementUserInteractions(comment.CreatedBy);

                transaction.Commit();
                return comment;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// Adds a comment to a post.
        /// </summary>
        /// <param name="comment">The comment to be added.</param>
        /// <returns>The created comment.</returns>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        public async Task<Comment> CommentAPost(Comment comment)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var post = context.Posts.FirstOrDefault(p => p.Id == comment.PostId);
                if (post == null)
                {
                    throw new NotFoundException("Post not found");
                }

                GreenitorDTO user = await greenitorDAO.GetUserByUsername(comment.CreatedBy);
                comment.Post = post;
                comment.ParentPostId = post.Id;
                comment.CreatedAt = DateTime.UtcNow;
                context.Comments.Add(comment);
                post.Interactions++;

                await context.SaveChangesAsync();
                await greenitorDAO.IncrementUserInteractions(comment.CreatedBy);

                transaction.Commit();
                return comment;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// Retrieves comment statistics for a specific user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user's comment statistics.</returns>
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

        /// <summary>
        /// Likes a comment.
        /// </summary>
        /// <param name="commentLike">The comment like object.</param>
        /// <returns>The created like entry.</returns>
        /// <exception cref="NotFoundException">Thrown if the comment is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if the user has already liked the comment.</exception>
        public async Task<CommentLike> LikeComment(CommentLike commentLike)
        {
            var comment = context.Comments.FirstOrDefault(c => c.Id == commentLike.CommentId);
            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }

            await greenitorDAO.GetUserByUsername(commentLike.User);

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

        /// <summary>
        /// Removes a like from a comment.
        /// </summary>
        /// <param name="commentLike">The comment like object.</param>
        /// <returns>The removed like entry.</returns>
        public async Task<CommentLike> UnLikeComment(CommentLike commentLike)
        {
            var comment = context.Comments.FirstOrDefault(c => c.Id == commentLike.CommentId);
            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }

            await greenitorDAO.GetUserByUsername(commentLike.User);

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

        /// <summary>
        /// Retrieves the number of likes for a given comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment.</param>
        /// <returns>The total number of likes.</returns>
        public async Task<int> GetNumberOfLikesFromCommentId(int commentId)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }
            return await context.CommentLikes.Where(cl => cl.CommentId == commentId).CountAsync();
        }

        /// <summary>
        /// Checks if a user has interacted with a comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment.</param>
        /// <param name="username">The username of the user.</param>
        /// <returns>1 if the user liked the comment, 0 otherwise.</returns>
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

        /// <summary>
        /// Retrieves a list of replies to a specific comment.
        /// </summary>
        /// <param name="commentId">The ID of the parent comment.</param>
        /// <returns>A list of comments that are replies to the specified comment.</returns>
        /// <exception cref="NotFoundException">Thrown if the specified comment is not found.</exception>
        public async Task<List<Comment>> GetCommentsByCommentId(int commentId)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment == null)
            {
                throw new NotFoundException("Comment not found");
            }
            return await context.Comments.Where(c => c.ParentCommentId == commentId)
                                         .Include(c => c.Replies)
                                         .ToListAsync();
        }
    }
}