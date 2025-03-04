using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    public interface PostDAO
    {
        public Task<Post> GetPostById(int id);
        public Task<Post> CreatePost(Post post);
    }
}