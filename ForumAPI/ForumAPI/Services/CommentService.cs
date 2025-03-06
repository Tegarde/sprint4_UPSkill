﻿using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using Microsoft.EntityFrameworkCore;

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

            if(parentComment.PostId != null) 
            {
                comment.ParentPostId = parentComment.PostId;

            }else  
            {
                comment.ParentPostId = parentComment.ParentPostId;
            }

            if (!comment.ParentPostId.HasValue)
            {
                throw new NotFoundException("Unnable to link to Post");
            }

            // Set created at
            comment.CreatedAt = DateTime.UtcNow;

      

            context.Comments.Add(comment);
            context.SaveChanges();

            return comment;
        }

        public async Task<Comment> CommentAnEvent(Comment comment){
            var ev = context.Events.FirstOrDefault(e => e.Id == comment.EventId);
            if(ev == null)
            {
                throw new NotFoundException("Event not found");
            }

            //Get user from greenitorDAO
            GreenitorDTO user = await greenitorDAO.GetUserByUsername(comment.CreatedBy);
            if(user == null)
            {
                throw new UserNotFoundException("User not found");
            }
            
            comment.Event = ev;
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> CommentAPost(Comment comment)
        {
            var post = context.Posts.FirstOrDefault(p => p.Id == comment.PostId);
            if(post == null)
            { throw new NotFoundException("Post not found"); }

            GreenitorDTO user = await greenitorDAO.GetUserByUsername(comment.CreatedBy);
            if(user == null)
            {
                throw new UserNotFoundException("User not found");
            }
            comment.Post = post;

            context.Comments.Add(comment);

            await context.SaveChangesAsync();

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

    }
}
