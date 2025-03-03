using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    public interface PostDAO
    {
        public Task<Post> CreatePost(Post post);
    }
}