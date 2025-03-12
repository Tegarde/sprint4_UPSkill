using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.DTOs.PostDTOs;
using ForumAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Interfaces
{
    /// <summary>
    /// Interface defining data access operations related to posts.
    /// </summary>
    public interface PostDAO
    {
        /// <summary>
        /// Retrieves all posts in the system.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="Post"/> objects.</returns>
        Task<List<Post>> GetAllPosts();

        /// <summary>
        /// Retrieves a post by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the post to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with the <see cref="Post"/> object corresponding to the provided post ID.</returns>
        Task<Post> GetPostById(int id);

        /// <summary>
        /// Retrieves a list of posts created by a specific user.
        /// </summary>
        /// <param name="username">The username of the user whose posts to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="Post"/> objects created by the specified user.</returns>
        Task<List<Post>> GetPostsByUser(string username);

        /// <summary>
        /// Retrieves a list of posts sorted by their creation date.
        /// </summary>
        /// <returns>A list of <see cref="Post"/> objects sorted by creation date.</returns>
        List<Post> GetPostSortedByDate();

        /// <summary>
        /// Retrieves the top N posts based on their interaction count.
        /// </summary>
        /// <param name="topN">The number of top posts to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a list of the top N <see cref="Post"/> objects based on interactions.</returns>
        Task<List<Post>> GetTopPostsByInteractions(int topN);

        /// <summary>
        /// Retrieves a list of posts marked as favorites by a specific user.
        /// </summary>
        /// <param name="username">The username of the user whose favorite posts to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="Post"/> objects marked as favorites by the specified user.</returns>
        Task<List<Post>> GetFavoritePosts(string username);

        /// <summary>
        /// Retrieves a list of posts within a specific date range.
        /// </summary>
        /// <param name="startDate">The start date of the date range.</param>
        /// <param name="endDate">The end date of the date range.</param>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="Post"/> objects within the specified date range.</returns>
        Task<List<Post>> GetPostsBetweenDates(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="post">The <see cref="Post"/> object containing the details of the new post.</param>
        /// <returns>A task representing the asynchronous operation, with the created <see cref="Post"/> object.</returns>
        Task<Post> CreatePost(Post post);

        /// <summary>
        /// Updates an existing post by its ID.
        /// </summary>
        /// <param name="postId">The ID of the post to update.</param>
        /// <param name="updatedPost">The updated <see cref="Post"/> object.</param>
        /// <returns>A task representing the asynchronous operation, with the updated <see cref="Post"/> object.</returns>
        Task<Post> UpdatePost(int postId, Post updatedPost);

        /// <summary>
        /// Adds a post to the favorites list of a specific user.
        /// </summary>
        /// <param name="postId">The ID of the post to add to favorites.</param>
        /// <param name="username">The username of the user adding the post to favorites.</param>
        /// <returns>A task representing the asynchronous operation, with the result of the operation.</returns>
        Task<ActionResult> AddPostToFavorites(int postId, string username);

        /// <summary>
        /// Removes a post from the favorites list of a specific user.
        /// </summary>
        /// <param name="postId">The ID of the post to remove from favorites.</param>
        /// <param name="username">The username of the user removing the post from favorites.</param>
        /// <returns>A task representing the asynchronous operation, with the result of the operation.</returns>
        Task<ActionResult> RemovePostFromFavorites(int postId, string username);

        /// <summary>
        /// Updates the status of a post.
        /// </summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="newStatus">The new status of the post (e.g., active, deleted).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdatePostStatus(int id, bool newStatus);

        /// <summary>
        /// Searches for posts by a given keyword in their content or title.
        /// </summary>
        /// <param name="keyword">The keyword to search for in posts.</param>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="Post"/> objects matching the keyword.</returns>
        Task<List<Post>> SearchPostsByKeyword(string keyword);

        /// <summary>
        /// Retrieves the "hotness score" of a post, based on its interactions.
        /// </summary>
        /// <param name="postId">The ID of the post to retrieve the hotness score for.</param>
        /// <returns>A task representing the asynchronous operation, with the hotness score of the post.</returns>
        Task<int> GetPostHotnessScore(int postId);

        /// <summary>
        /// Retrieves the top N hottest posts.
        /// </summary>
        /// <param name="topN">The number of top hottest posts to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a list of the top N hottest <see cref="Post"/> objects.</returns>
        Task<List<Post>> GetHottestPosts(int topN);

        /// <summary>
        /// Retrieves the top N hottest posts from the past month.
        /// </summary>
        /// <param name="topN">The number of top hottest posts to retrieve from the last month.</param>
        /// <returns>A task representing the asynchronous operation, with a list of the hottest posts from the last month.</returns>
        Task<List<Post>> GetHottestPostsFromLastMonth(int topN);

        /// <summary>
        /// Retrieves the top N hottest posts from the past day.
        /// </summary>
        /// <param name="topN">The number of top hottest posts to retrieve from the last day.</param>
        /// <returns>A task representing the asynchronous operation, with a list of the hottest posts from the last day.</returns>
        Task<List<Post>> GetHottestPostsFromLastDay(int topN);

        /// <summary>
        /// Retrieves notifications for a specific user, related to posts.
        /// </summary>
        /// <param name="username">The username of the user for whom to retrieve post-related notifications.</param>
        /// <returns>A task representing the asynchronous operation, with a list of <see cref="PostNotificationDTO"/> objects for the user.</returns>
        Task<List<Post>> GetNotificationsByUser(string username);

        /// <summary>
        /// Resets the interaction count for a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post whose interaction count is to be reset.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ResetPostInteractionCount(int postId);

        /// <summary>
        /// Retrieves post statistics for a specific user.
        /// </summary>
        /// <param name="username">The username of the user for whom to retrieve post statistics.</param>
        /// <returns>A task representing the asynchronous operation, with <see cref="GreenitorStatisticsDTO"/> containing the user's post statistics.</returns>
        Task<GreenitorStatisticsDTO> GetPostStatisticsByUsername(string username);

        /// <summary>
        /// Likes a specific post.
        /// </summary>
        /// <param name="postLike">The <see cref="PostLike"/> object containing the post ID and user details for the like.</param>
        /// <returns>A task representing the asynchronous operation, with the created <see cref="PostLike"/> object.</returns>
        Task<PostLike> LikePost(PostLike postLike);

        /// <summary>
        /// Unlikes a specific post.
        /// </summary>
        /// <param name="postLike">The <see cref="PostLike"/> object containing the post ID and user details for the unlike.</param>
        /// <returns>A task representing the asynchronous operation, with the removed <see cref="PostLike"/> object.</returns>
        Task<PostLike> UnlikePost(PostLike postLike);

        /// <summary>
        /// Dislikes a specific post.
        /// </summary>
        /// <param name="postDislike">The <see cref="PostDislike"/> object containing the post ID and user details for the dislike.</param>
        /// <returns>A task representing the asynchronous operation, with the created <see cref="PostDislike"/> object.</returns>
        Task<PostDislike> DislikePost(PostDislike postDislike);

        /// <summary>
        /// Undislikes a specific post.
        /// </summary>
        /// <param name="postDislike">The <see cref="PostDislike"/> object containing the post ID and user details for the undislike.</param>
        /// <returns>A task representing the asynchronous operation, with the removed <see cref="PostDislike"/> object.</returns>
        Task<PostDislike> UndislikePost(PostDislike postDislike);

        /// <summary>
        /// Retrieves the interactions (likes, dislikes) for a specific post by a specific user.
        /// </summary>
        /// <param name="id">The post ID.</param>
        /// <param name="username">The username of the user whose interactions are being queried.</param>
        /// <returns>A task representing the asynchronous operation, with the interaction count for the post by the user.</returns>
        Task<int> GetPostInteractionsByUser(int id, string username);

        /// <summary>
        /// Retrieves the like and dislike counts for a specific post.
        /// </summary>
        /// <param name="postId">The post ID.</param>
        /// <returns>A task representing the asynchronous operation, with a <see cref="PostInteractionsDTO"/> containing the like and dislike counts.</returns>
        Task<PostInteractionsDTO> GetLikesAndDislikesByPostId(int postId);

        /// <summary>
        /// Retrieves the favorite count for a specific post by a specific user.
        /// </summary>
        /// <param name="postFavorite">The <see cref="PostFavorite"/> object containing the post ID and user details for the favorite count.</param>
        /// <returns>A task representing the asynchronous operation, with the favorite count for the specified post and user.</returns>
        Task<int> GetPostFavoriteByUsername(PostFavorite postFavorite);
    }
}