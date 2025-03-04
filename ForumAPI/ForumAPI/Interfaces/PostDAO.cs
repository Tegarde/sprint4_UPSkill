using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    public interface PostDAO
    {
        public Post GetPostById(int id);
        public Post CreatePost(Post post);
    }
}