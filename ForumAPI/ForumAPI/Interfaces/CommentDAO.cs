using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    /// <summary>
    /// Interface defining data access operations related to comments.
    /// </summary>
    public interface CommentDAO
    {
        /// <summary>
        /// Adds a comment to another comment.
        /// </summary>
        /// <param name="comment">The comment being added.</param>
        /// <returns>A task representing the asynchronous operation, with the added <see cref="Comment"/> as the result.</returns>
        Task<Comment> CommentAComment(Comment comment);

        /// <summary>
        /// Adds a comment to an event.
        /// </summary>
        /// <param name="comment">The comment being added to the event.</param>
        /// <returns>A task representing the asynchronous operation, with the added <see cref="Comment"/> as the result.</returns>
        Task<Comment> CommentAnEvent(Comment comment);

        /// <summary>
        /// Adds a comment to a post.
        /// </summary>
        /// <param name="comment">The comment being added to the post.</param>
        /// <returns>A task representing the asynchronous operation, with the added <see cref="Comment"/> as the result.</returns>
        Task<Comment> CommentAPost(Comment comment);

        /// <summary>
        /// Retrieves the comment statistics for a user based on their username.
        /// </summary>
        /// <param name="username">The username of the user for whom to fetch comment statistics.</param>
        /// <returns>A task representing the asynchronous operation, with a <see cref="GreenitorStatisticsDTO"/> as the result.</returns>
        Task<GreenitorStatisticsDTO> GetCommentStatisticsByUsername(string username);

        /// <summary>
        /// Unlikes a comment by removing the like from a specific comment.
        /// </summary>
        /// <param name="commentLike">The comment like object containing the comment ID and the user who is unliking it.</param>
        /// <returns>A task representing the asynchronous operation, with the removed <see cref="CommentLike"/> as the result.</returns>
        Task<CommentLike> UnLikeComment(CommentLike commentLike);

        /// <summary>
        /// Likes a comment, adding a like from the specified user.
        /// </summary>
        /// <param name="commentLike">The comment like object containing the comment ID and the user who is liking it.</param>
        /// <returns>A task representing the asynchronous operation, with the added <see cref="CommentLike"/> as the result.</returns>
        Task<CommentLike> LikeComment(CommentLike commentLike);

        /// <summary>
        /// Retrieves the number of likes a comment has received.
        /// </summary>
        /// <param name="commentId">The ID of the comment for which to fetch the like count.</param>
        /// <returns>A task representing the asynchronous operation, with the number of likes as the result.</returns>
        Task<int> GetNumberOfLikesFromCommentId(int commentId);

        /// <summary>
        /// Retrieves the number of interactions (likes, dislikes, etc.) a user has with a specific comment.
        /// </summary>
        /// <param name="commentId">The ID of the comment for which to fetch interactions.</param>
        /// <param name="username">The username of the user whose interactions with the comment are being fetched.</param>
        /// <returns>A task representing the asynchronous operation, with the number of interactions as the result.</returns>
        Task<int> GetCommentInteractionsByUser(int commentId, string username);

        /// <summary>
        /// Retrieves a list of comments by their parent comment ID.
        /// </summary>
        /// <param name="commentId">The parent comment ID for which to fetch all associated comments.</param>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="Comment"/> objects as the result.</returns>
        Task<List<Comment>> GetCommentsByCommentId(int commentId);
    }
}