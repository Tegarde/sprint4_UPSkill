using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;
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

        public async Task<List<Post>> GetAllPosts()
        {
            var posts = await context.Posts
                .Include(p => p.Comments)
                .ThenInclude(p => p.Replies)
                .Where(p => p.Status)
                .ToListAsync();

            return posts;
        }

        /// <summary>
        /// Retrieves a post by its ID and ensures it exists.
        /// </summary>
        /// <param name="id">The post ID.</param>
        /// <returns>The post if found, otherwise throws an exception.</returns>
        public async Task<Post> GetPostById(int id)
        {
            var post = await context.Posts
                .Include(p => p.Comments)
                .Include(l => l.LikedBy)
                .Include(d => d.DislikedBy)
                .Where(p => p.Status)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }

            return post;
        }

        public async Task<List<Post>> GetPostsByUser(string username)
        {

            GreenitorDTO? user = await greenitorClient.GetUserByUsername(username);
            if (user == null)
            {
                throw new Exception("User does not exist.");
            }


            var posts = await context.Posts
                .Where(p => p.CreatedBy == username && p.Status)
                .ToListAsync();

            return posts;
        }

        public List<Post> GetPostSortedByDate()
        {
            return context.Posts
           .Where(p => p.Status)
           .OrderByDescending(p => p.CreatedAt)
           .ToList();
        }

        public async Task<List<Post>> GetTopPostsByInteractions(int topN)
        {
            return await context.Posts
                .Where(p => p.Status)
                .OrderByDescending(p => p.LikedBy.Count + p.Comments.Count + p.FavoritedBy.Count)
                .Take(topN)
                .ToListAsync();
        }

        public async Task<List<Post>> GetFavoritePosts([FromBody] string username)
        {
            GreenitorDTO? user = await greenitorClient.GetUserByUsername(username);
            if (user == null)
            {
                throw new Exception("User does not exist.");
            }

            var favoritePosts = await context.PostFavorites
                .Where(pf => pf.User == username)
                .Include(pf => pf.Post)
                .ThenInclude(p => p.Comments)
                .ToListAsync();

            return favoritePosts.Select(pf => pf.Post).ToList();
        }
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

        public async Task<ActionResult> AddPostToFavorites(int postId, string username)
        {
            var post = await GetPostById(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }

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

        public async Task<ActionResult> RemovePostFromFavorites(int postId, string username)
        {
            var favorite = await context.PostFavorites
                .FirstOrDefaultAsync(pf => pf.PostId == postId && pf.User == username);
            if (favorite == null)
            {
                throw new KeyNotFoundException("Favorite not found.");
            }
            context.PostFavorites.Remove(favorite);
            await context.SaveChangesAsync();
            await greenitorClient.DecrementUserInteractions(favorite.User);
            return new OkResult();
        }

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

        public async Task<List<Post>> SearchPostsByKeyword(string keyword)
        {
            var posts = await context.Posts
                .Where(p => (p.Title.Contains(keyword) || p.Content.Contains(keyword)) && p.Status)
                .ToListAsync();
            return posts;
        }


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

            if (post == null){
                throw new NotFoundException("Post not found");           
            }

            return post.Likes + post.Favorites + post.DirectComments + post.IndirectComments;
        }

        public async Task<List<Post>> GetHottestPosts(int topN)
        {
            return await context.Posts
                .Where(p => p.Status)
                .OrderByDescending(p =>
                    p.LikedBy.Count +
                    p.FavoritedBy.Count +
                    p.Comments.Count +
                    context.Comments.Count(c => c.ParentPostId == p.Id)) // Ordena por HOTNESS
                .Take(topN)
                //.AsSplitQuery() // Otimiza a consulta
                //.Include(p => p.Comments)
                //.Include(p => p.LikedBy)
                //.Include(p => p.FavoritedBy)
                .ToListAsync();
        }

        public async Task<List<Post>> GetNotificationsByUser(string username)
        {
            await greenitorClient.GetUserByUsername(username);
            
            var posts = await context.Posts
                .Where(p => p.CreatedBy == username && p.Status && p.Interactions > 0)
                .ToListAsync();

            return posts;
        }

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

        public async Task<PostLike> LikePost(PostLike postLike)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postLike.PostId);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            GreenitorDTO user = await greenitorClient.GetUserByUsername(postLike.User);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            PostLike? like = await context.PostLikes.FirstOrDefaultAsync(pl => pl.PostId == postLike.PostId && pl.User == postLike.User);

            if (like == null)
            {
                var dislike = await context.PostDislikes.FirstOrDefaultAsync(pl => pl.PostId == postLike.PostId && pl.User == postLike.User);
                if(dislike != null)
                {
                    context.PostDislikes.Remove(dislike);
                }
                postLike.Post = post;
                context.PostLikes.Add(postLike);
                await context.SaveChangesAsync();
                await greenitorClient.IncrementUserInteractions(postLike.User);
                return postLike;
            }
            else
            {
                throw new ArgumentException("User already liked this post.");
            }

        }

        public async Task<PostDislike> DislikePost(PostDislike postDislike)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postDislike.PostId);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            GreenitorDTO user = await greenitorClient.GetUserByUsername(postDislike.User);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

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
                
                await context.SaveChangesAsync();
                await greenitorClient.IncrementUserInteractions(postDislike.User);
                return postDislike;
            }
            else
            {
                throw new ArgumentException("User already disliked this post.");
            }
        }

        public async Task<PostLike> UnlikePost(PostLike postLike)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postLike.PostId);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            GreenitorDTO user = await greenitorClient.GetUserByUsername(postLike.User);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

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

        public async Task<PostDislike> UndislikePost(PostDislike postDislike)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == postDislike.PostId);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            GreenitorDTO user = await greenitorClient.GetUserByUsername(postDislike.User);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

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

        public async Task<int> GetPostInteractionsByUser(int postId, string username)
        {
            var user = await greenitorClient.GetUserByUsername(username);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }

            var postExists = await context.Posts.AnyAsync(p => p.Id == postId);
            if (!postExists)
            {
                throw new NotFoundException("Post not found.");
            }

            
            if (await context.PostLikes.AnyAsync(pl => pl.PostId == postId && pl.User == username))
            {
                return 1; // Retorna 1 se o utilizador deu Like
            }

            
            if (await context.PostDislikes.AnyAsync(pd => pd.PostId == postId && pd.User == username))
            {
                return -1; // Retorna -1 se o utilizador deu Dislike
            }

            return 0; // Retorna 0 se não houver interação
        }





    }
}
