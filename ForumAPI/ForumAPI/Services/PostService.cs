using ForumAPI.Data;
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
                throw new KeyNotFoundException($"Post with ID {id} not found.");
            }

            return post;
        }

        public async Task<Post> CreatePost(Post post)
        {
            GreenitorDTO? user = await greenitorClient.GetUserByUsername(post.CreatedBy);

            if (user == null)
            {
                throw new ArgumentException($"User '{post.CreatedBy}' does not exist. Cannot create post.");
            }

            context.Posts.Add(post);
            await context.SaveChangesAsync();

            return post;
        }

        public async Task<ActionResult> AddPostToFavorites(int postId, string username)
        {
            var post = await GetPostById(postId);
            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID {postId} not found.");
            }

            var favorite = new PostFavorite
            {
                PostId = postId,
                User = username
            };

            context.PostFavorites.Add(favorite);
            await context.SaveChangesAsync();
            return new OkObjectResult($"Post with ID {postId} added to favorites for user '{username}'.");
        }

        public async Task<ActionResult> RemovePostFromFavorites(int postId, string username)
        {
            var favorite = await context.PostFavorites
                .FirstOrDefaultAsync(pf => pf.PostId == postId && pf.User == username);
            if (favorite == null)
            {
                throw new KeyNotFoundException($"Favorite for post ID {postId} and user '{username}' not found.");
            }
            context.PostFavorites.Remove(favorite);
            await context.SaveChangesAsync();
            return new OkObjectResult($"Post with ID {postId} removed from favorites for user '{username}'.");
        }


        // refazer SD US02
        public async Task UpdatePostStatus(int id, bool newStatus)
        {
            var post = await GetPostById(id);
            if (post == null)
            {
                throw new KeyNotFoundException($"Post with ID {id} not found.");
            }

            if (post.Status == newStatus)
            {
                throw new InvalidOperationException("New status must be different from the current status.");
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
