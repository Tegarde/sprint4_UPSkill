﻿using ForumAPI.CustomExceptions;
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

        public async Task<List<Post>> GetAllPosts()
        {
            var posts = await context.Posts
                .Include(p => p.Comments)
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
                .Include(p => p.Comments)

                .ToListAsync();

            return posts;
        }

        public List<Post> GetPostSortedByDate()
        {
            return context.Posts
           .Include(p => p.Comments)
           .Where(p => p.Status)
           .OrderByDescending(p => p.CreatedAt)
           .ToList();
        }

        public async Task<List<Post>> GetTopPostsByInteractions(int topN)
        {
            return await context.Posts
                .Include(p => p.Comments)
                .Include(p => p.LikedBy)
                .Include(p => p.FavoritedBy)
                .Where(p => p.Status)
                .OrderByDescending(p => p.LikedBy.Count + p.Comments.Count + p.FavoritedBy.Count)
                .Take(topN)
                .ToListAsync();
        }

        public async Task<List<Post>> GetPostsBetweenDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("The start date must be earlier than the end date.");
            }

            return await context.Posts
                .Include(p => p.Comments)
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate && p.Status)
                .ToListAsync();
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
            return new OkResult();
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
