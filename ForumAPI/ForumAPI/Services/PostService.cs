using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.DTOs.PostDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumAPI.Services
{
    public class PostService : PostDAO
    {
        private readonly DataContext context;
        private readonly GreenitorDAO greenitorClient;
        private readonly CategoryDAO categoryClient;

        public PostService(DataContext context, GreenitorDAO greenitorClient, CategoryDAO categoryClient)
        {
            this.context = context;
            this.greenitorClient = greenitorClient;
            this.categoryClient = categoryClient;
        }

        /// <summary>
        /// Retrieves all posts, including their comments and replies.
        /// </summary>
        /// <returns>A list of all posts.</returns>
        public async Task<List<Post>> GetAllPosts()
        {
            var posts = await context.Posts
                .Include(p => p.Comments)
                .ThenInclude(p => p.Replies)
                .ToListAsync();

            return posts;
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the post.</param>
        /// <returns>The post if found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the post is not found.</exception>
        public async Task<Post> GetPostById(int id)
        {
            var post = await context.Posts
                .Include(p => p.Comments)
                .ThenInclude(p => p.Replies)
                .Include(l => l.LikedBy)
                .Include(d => d.DislikedBy)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }

            return post;
        }

        /// <summary>
        /// Retrieves all posts created by a specific user.
        /// </summary>
        /// <param name="username">The username of the post creator.</param>
        /// <returns>A list of posts created by the specified user.</returns>
        public async Task<List<Post>> GetPostsByUser(string username)
        {
            await greenitorClient.GetUserByUsername(username);

            var posts = await context.Posts
                .Include(p => p.Comments)
                .ThenInclude(p => p.Replies)
                .Where(p => p.CreatedBy == username && p.Status)
                .ToListAsync();

            return posts;
        }

        /// <summary>
        /// Retrieves posts sorted by creation date, from newest to oldest.
        /// </summary>
        /// <returns>A list of posts sorted by creation date.</returns>
        public List<Post> GetPostSortedByDate()
        {
            return context.Posts
                .Where(p => p.Status)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }

        /// <summary>
        /// Retrieves the top N posts based on interactions (likes, comments, favorites, and dislikes).
        /// </summary>
        /// <param name="topN">The number of top posts to retrieve.</param>
        /// <returns>A list of the top interacted posts.</returns>
        public async Task<List<Post>> GetTopPostsByInteractions(int topN)
        {
            return await context.Posts
                .Where(p => p.Status)
                .OrderByDescending(p => p.LikedBy.Count + p.Comments.Count + p.FavoritedBy.Count + p.DislikedBy.Count)
                .Take(topN)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a user's favorite posts.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>A list of the user's favorite posts.</returns>
        public async Task<List<Post>> GetFavoritePosts([FromBody] string username)
        {
            await greenitorClient.GetUserByUsername(username);

            var favoritePosts = await context.PostFavorites
                .Where(pf => pf.User == username)
                .Include(pf => pf.Post)
                .ToListAsync();

            return favoritePosts.Select(pf => pf.Post).ToList();
        }

        /// <summary>
        /// Retrieves posts created within a specific date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A list of posts within the specified date range.</returns>
        /// <exception cref="ArgumentException">Thrown if the start date is later than the end date.</exception>
        public async Task<List<Post>> GetPostsBetweenDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("The start date must be earlier than the end date.");
            }
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            return await context.Posts
                .Include(p => p.Comments)
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate && p.Status)
                .ToListAsync();
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="post">The post object to be created.</param>
        /// <returns>The created post.</returns>
        public async Task<Post> CreatePost(Post post)
        {
            await greenitorClient.GetUserByUsername(post.CreatedBy);

            await categoryClient.GetCategoryByDescription(post.Category);

            await greenitorClient.IncrementUserInteractions(post.CreatedBy);

            context.Posts.Add(post);
            await context.SaveChangesAsync();
            await greenitorClient.IncrementUserInteractions(post.CreatedBy);

            return post;
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="postId">The ID of the post to update.</param>
        /// <param name="updatedPost">The updated post object.</param>
        /// <returns>The updated post.</returns>
        /// <exception cref="ArgumentException">Thrown if the post ID is invalid.</exception>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if the user is not the post creator.</exception>
        public async Task<Post> UpdatePost(int postId, Post updatedPost)
        {
            if (postId != updatedPost.Id)
            {
                throw new ArgumentException("Invalid Post Id");
            }

            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
            {
                throw new NotFoundException("Post not found");
            }

            if (!post.CreatedBy.Equals(updatedPost.CreatedBy))
            {
                throw new UnauthorizedAccessException("You are not authorized to update this post.");
            }

            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;

            await context.SaveChangesAsync();
            return post;
        }

        /// <summary>
        /// Adds a post to a user's favorites.
        /// </summary>
        /// <param name="postId">The ID of the post to add to favorites.</param>
        /// <param name="username">The username of the user adding the post to favorites.</param>
        /// <returns>An ActionResult indicating success.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the post is not found.</exception>
        public async Task<ActionResult> AddPostToFavorites(int postId, string username)
        {
            var post = await GetPostById(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }

            await greenitorClient.GetUserByUsername(username);

            var favorite = new PostFavorite
            {
                PostId = postId,
                User = username
            };

            context.PostFavorites.Add(favorite);
            await context.SaveChangesAsync();
            await greenitorClient.IncrementUserInteractions(favorite.User);
            return new OkResult();
        }

        /// <summary>
        /// Removes a post from a user's favorites.
        /// </summary>
        /// <param name="postId">The ID of the post to remove from favorites.</param>
        /// <param name="username">The username of the user removing the post from favorites.</param>
        /// <returns>An ActionResult indicating success.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the favorite entry is not found.</exception>
        public async Task<ActionResult> RemovePostFromFavorites(int postId, string username)
        {
            var favorite = await context.PostFavorites
                .FirstOrDefaultAsync(pf => pf.PostId == postId && pf.User == username);
            if (favorite == null)
            {
                throw new KeyNotFoundException("Favorite not found.");
            }

            await greenitorClient.GetUserByUsername(username);

            context.PostFavorites.Remove(favorite);
            await context.SaveChangesAsync();
            await greenitorClient.DecrementUserInteractions(favorite.User);
            return new OkResult();
        }

        /// <summary>
        /// Updates the status of a post.
        /// </summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="newStatus">The new status to assign to the post.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the post is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the new status is the same as the current status.</exception>
        public async Task UpdatePostStatus(int id, bool newStatus)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                throw new KeyNotFoundException("Post not found");
            }

            if (post.Status == newStatus)
            {
                throw new InvalidOperationException("New status must be different from the current status");
            }

            post.Status = newStatus;
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Searches for posts containing a specific keyword in their title, content, or username.
        /// </summary>
        /// <param name="keyword">The keyword to search for.</param>
        /// <returns>A list of posts matching the keyword.</returns>
        public async Task<List<Post>> SearchPostsByKeyword(string keyword)
        {
            var posts = await context.Posts
                .Where(p => (p.Title.Contains(keyword) || p.Content.Contains(keyword) || p.CreatedBy == keyword) && p.Status)
                .ToListAsync();
            return posts;
        }

        /// <summary>
        /// Calculates the hotness score of a post based on interactions.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>The calculated hotness score.</returns>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        public async Task<int> GetPostHotnessScore(int postId)
        {
            var post = await context.Posts
                .Where(p => p.Id == postId)
                .Select(p => new
                {
                    Likes = p.LikedBy.Count,
                    Favorites = p.FavoritedBy.Count,
                    DirectComments = p.Comments.Count,
                    IndirectComments = context.Comments.Count(c => c.ParentPostId == postId)
                })
                .FirstOrDefaultAsync();

            if (post == null)
            {
                throw new NotFoundException("Post not found");
            }

            return post.Likes + post.Favorites + post.DirectComments + post.IndirectComments;
        }

        /// <summary>
        /// Retrieves the hottest posts based on interactions.
        /// </summary>
        /// <param name="topN">The number of top posts to retrieve.</param>
        /// <returns>A list of the hottest posts.</returns>
        public async Task<List<Post>> GetHottestPosts(int topN)
        {
            return await context.Posts
                .Where(p => p.Status)
                .OrderByDescending(p =>
                    p.LikedBy.Count +
                    p.FavoritedBy.Count +
                    p.Comments.Count +
                    context.Comments.Count(c => c.ParentPostId == p.Id) +
                    p.DislikedBy.Count)
                .Take(topN)
                .AsSplitQuery()
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves the hottest posts from the last month based on user interactions.
        /// </summary>
        /// <param name="topN">The number of top posts to retrieve.</param>
        /// <returns>A list of the hottest posts from the last month.</returns>
        public async Task<List<Post>> GetHottestPostsFromLastMonth(int topN)
        {
            DateTime lastMonth = DateTime.UtcNow.AddMonths(-1);

            var hottestPosts = await context.Posts
                .Where(p => p.Status)
                .Where(p =>
                    p.CreatedAt >= lastMonth ||
                    p.LikedBy.Any(like => like.LikedAt >= lastMonth) ||
                    p.Comments.Any(comment => comment.CreatedAt >= lastMonth) ||
                    p.DislikedBy.Any(dislike => dislike.DislikedAt >= lastMonth))
                .Select(p => new
                {
                    Post = p,
                    InteractionScore = p.LikedBy.Count +
                                       p.FavoritedBy.Count +
                                       p.Comments.Count +
                                       context.Comments.Count(c => c.ParentPostId == p.Id)
                })
                .OrderByDescending(p => p.InteractionScore)
                .Take(topN)
                .Select(p => p.Post)
                .ToListAsync();

            return hottestPosts;
        }

        /// <summary>
        /// Retrieves the hottest posts from the last day based on user interactions.
        /// </summary>
        /// <param name="topN">The number of top posts to retrieve.</param>
        /// <returns>A list of the hottest posts from the last day.</returns>
        public async Task<List<Post>> GetHottestPostsFromLastDay(int topN)
        {
            DateTime lastDay = DateTime.UtcNow.AddDays(-1);

            var hottestPosts = await context.Posts
                .Where(p => p.Status)
                .Where(p =>
                    p.CreatedAt >= lastDay ||
                    p.LikedBy.Any(like => like.LikedAt >= lastDay) ||
                    p.Comments.Any(comment => comment.CreatedAt >= lastDay) ||
                    p.DislikedBy.Any(dislike => dislike.DislikedAt >= lastDay))
                .Select(p => new
                {
                    Post = p,
                    InteractionScore = p.LikedBy.Count +
                                       p.FavoritedBy.Count +
                                       p.Comments.Count +
                                       context.Comments.Count(c => c.ParentPostId == p.Id)
                })
                .OrderByDescending(p => p.InteractionScore)
                .Take(topN)
                .Select(p => p.Post)
                .ToListAsync();

            return hottestPosts;
        }

        /// <summary>
        /// Retrieves posts with interactions for a specific user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>A list of posts with interactions.</returns>
        public async Task<List<Post>> GetNotificationsByUser(string username)
        {
            await greenitorClient.GetUserByUsername(username);

            var posts = await context.Posts
                .Where(p => p.CreatedBy == username && p.Status && p.Interactions > 0)
                .ToListAsync();

            return posts;
        }

        /// <summary>
        /// Resets the interaction count of a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        public async Task ResetPostInteractionCount(int postId)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            post.Interactions = 0;
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves statistics related to posts for a specific user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user's post statistics.</returns>
        public async Task<GreenitorStatisticsDTO> GetPostStatisticsByUsername(string username)
        {
            await greenitorClient.GetUserByUsername(username);

            var likesInPosts = await context.PostLikes
                .Where(pl => pl.User == username).CountAsync();

            var dislikesInPosts = await context.PostDislikes
                .Where(pl => pl.User == username).CountAsync();

            var favoritePosts = await context.PostFavorites
                .Where(pf => pf.User == username).CountAsync();

            var posts = await context.Posts
                .Where(p => p.CreatedBy == username).CountAsync();

            return new GreenitorStatisticsDTO
            {
                LikesInPosts = likesInPosts,
                DislikesInPosts = dislikesInPosts,
                FavoritePosts = favoritePosts,
                Posts = posts
            };
        }

        /// <summary>
        /// Registers a like for a post.
        /// </summary>
        /// <param name="postLike">The like object.</param>
        /// <returns>The created like entry.</returns>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if the user has already liked the post.</exception>
        public async Task<PostLike> LikePost(PostLike postLike)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postLike.PostId);

                if (post == null)
                {
                    throw new NotFoundException("Post not found.");
                }

                await greenitorClient.GetUserByUsername(postLike.User);

                PostLike? like = await context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postLike.PostId && pl.User == postLike.User);

                if (like == null)
                {
                    var dislike = await context.PostDislikes.FirstOrDefaultAsync(pl => pl.PostId == postLike.PostId && pl.User == postLike.User);
                    if (dislike != null)
                    {
                        context.PostDislikes.Remove(dislike);
                    }
                    postLike.Post = post;
                    context.PostLikes.Add(postLike);
                    post.Interactions++;
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    if (dislike == null)
                    {
                        await greenitorClient.IncrementUserInteractions(postLike.User);
                    }

                    return postLike;
                }
                else
                {
                    throw new ArgumentException("User already liked this post.");
                }
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
        }

        /// <summary>
        /// Registers a dislike for a post.
        /// </summary>
        /// <param name="postDislike">The dislike object.</param>
        /// <returns>The created dislike entry.</returns>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if the user has already disliked the post.</exception>
        public async Task<PostDislike> DislikePost(PostDislike postDislike)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postDislike.PostId);

                if (post == null)
                {
                    throw new NotFoundException("Post not found.");
                }

                await greenitorClient.GetUserByUsername(postDislike.User);

                PostDislike? dislike = await context.PostDislikes.FirstOrDefaultAsync(pl => pl.PostId == postDislike.PostId && pl.User == postDislike.User);

                if (dislike == null)
                {
                    var like = await context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postDislike.PostId && pl.User == postDislike.User);
                    if (like != null)
                    {
                        context.PostLikes.Remove(like);
                    }
                    postDislike.Post = post;
                    context.PostDislikes.Add(postDislike);
                    post.Interactions++;
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    if (like == null)
                    {
                        await greenitorClient.IncrementUserInteractions(postDislike.User);
                    }

                    return postDislike;
                }
                else
                {
                    throw new ArgumentException("User already disliked this post.");
                }
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
        }

        /// <summary>
        /// Removes a like from a post.
        /// </summary>
        /// <param name="postLike">The like object containing the post ID and user information.</param>
        /// <returns>The removed like entry.</returns>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if the user has not liked the post.</exception>
        public async Task<PostLike> UnlikePost(PostLike postLike)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postLike.PostId);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            await greenitorClient.GetUserByUsername(postLike.User);

            PostLike? like = await context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postLike.PostId && pl.User == postLike.User);

            if (like != null)
            {
                context.PostLikes.Remove(like);
                await context.SaveChangesAsync();
                await greenitorClient.DecrementUserInteractions(postLike.User);
                return like;
            }
            else
            {
                throw new ArgumentException("User has not liked this post.");
            }
        }

        /// <summary>
        /// Removes a dislike from a post.
        /// </summary>
        /// <param name="postDislike">The dislike object containing the post ID and user information.</param>
        /// <returns>The removed dislike entry.</returns>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if the user has not disliked the post.</exception>
        public async Task<PostDislike> UndislikePost(PostDislike postDislike)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postDislike.PostId);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            await greenitorClient.GetUserByUsername(postDislike.User);

            PostDislike? dislike = await context.PostDislikes.FirstOrDefaultAsync(pl => pl.PostId == postDislike.PostId && pl.User == postDislike.User);

            if (dislike != null)
            {
                context.PostDislikes.Remove(dislike);
                await context.SaveChangesAsync();
                await greenitorClient.DecrementUserInteractions(postDislike.User);
                return dislike;
            }
            else
            {
                throw new ArgumentException("User has not disliked this post.");
            }
        }

        /// <summary>
        /// Retrieves the interaction type (like, dislike, or none) of a user with a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="username">The username of the user.</param>
        /// <returns>
        /// 1 if the user has liked the post, -1 if the user has disliked the post, and 0 if there is no interaction.
        /// </returns>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        public async Task<int> GetPostInteractionsByUser(int postId, string username)
        {
            await greenitorClient.GetUserByUsername(username);

            var postExists = await context.Posts.AnyAsync(p => p.Id == postId);
            if (!postExists)
            {
                throw new NotFoundException("Post not found.");
            }

            if (await context.PostLikes.AnyAsync(pl => pl.PostId == postId && pl.User == username))
            {
                return 1; // User has liked the post
            }

            if (await context.PostDislikes.AnyAsync(pd => pd.PostId == postId && pd.User == username))
            {
                return -1; // User has disliked the post
            }

            return 0; // No interaction
        }

        /// <summary>
        /// Retrieves the number of likes and dislikes for a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>An object containing the number of likes and dislikes.</returns>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        public async Task<PostInteractionsDTO> GetLikesAndDislikesByPostId(int postId)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            int postLikes = await context.PostLikes.Where(pl => pl.PostId == postId).CountAsync();
            int postDislikes = await context.PostDislikes.Where(pl => pl.PostId == postId).CountAsync();

            return new PostInteractionsDTO { Likes = postLikes, Dislikes = postDislikes };
        }

        /// <summary>
        /// Checks if a user has favorited a specific post.
        /// </summary>
        /// <param name="postFavorite">The object containing post ID and username.</param>
        /// <returns>1 if the post is favorited by the user, otherwise 0.</returns>
        /// <exception cref="NotFoundException">Thrown if the post is not found.</exception>
        public async Task<int> GetPostFavoriteByUsername(PostFavorite postFavorite)
        {
            await greenitorClient.GetUserByUsername(postFavorite.User);

            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postFavorite.PostId);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            var postFav = await context.PostFavorites.FirstOrDefaultAsync(p => p.PostId == postFavorite.PostId && p.User == postFavorite.User);

            return (postFav != null) ? 1 : 0;
        }
    }
}
