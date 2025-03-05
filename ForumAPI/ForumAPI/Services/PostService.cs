using ForumAPI.Data;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumAPI.Services
{
    public class PostService : PostDAO
    {
        private readonly DataContext context;
        private readonly GreenitorDAO greenitorClient;

        public PostService(DataContext context, GreenitorDAO greenitorClient)
        {
            this.context = context;
            this.greenitorClient = greenitorClient;
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
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }

            return post;
        }

        public async Task<Post> CreatePost(Post post)
        {
            GreenitorDTO? user = await greenitorClient.GetUserByUsername(post.CreatedBy);

            if (user == null)
            {
                throw new Exception("User does not exist. Cannot create post.");
            }

            context.Posts.Add(post);
            await context.SaveChangesAsync();

            return post;
        }

        public async Task AddPostToFavorites(int postId, string username)
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
        }

        public async Task RemovePostFromFavorites(int postId, string username)
        {
            var favorite = await context.PostFavorites
                .FirstOrDefaultAsync(pf => pf.PostId == postId && pf.User == username);
            if (favorite == null)
            {
                throw new KeyNotFoundException("Favorite not found.");
            }
            context.PostFavorites.Remove(favorite);
            await context.SaveChangesAsync();
        }

        // refazer SD US02
        public async Task UpdatePostStatus(int id, bool newStatus, string userRole)
        {
            if (userRole != "Moderator")
            {
                throw new UnauthorizedAccessException("Only users with the role 'Moderator' can update the post status.");
            }

            var post = await GetPostById(id);
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
    }
}
