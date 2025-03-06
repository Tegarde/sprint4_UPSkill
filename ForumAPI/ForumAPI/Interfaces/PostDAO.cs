using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    public interface PostDAO
    {
        Post GetPostById(int id);
        Task<Post> CreatePost(Post post);
        Task UpdatePostStatus(int id, bool newStatus, string userRole);

        Task<List<Post>> SearchPostsByKeyword(string keyword);
    }
}
