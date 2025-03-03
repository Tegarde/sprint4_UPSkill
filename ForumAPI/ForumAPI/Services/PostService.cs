using ForumAPI.Data;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Models;

namespace ForumAPI.Services
{
    public class PostService : PostDAO
    {
        private readonly DataContext context;
        private readonly GreenitorClient greenitorClient;

        public PostService(DataContext context, HttpClient httpClient, GreenitorClient greenitorClient)
        {
            this.context = context;
            this.greenitorClient = greenitorClient;
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
    }
}
