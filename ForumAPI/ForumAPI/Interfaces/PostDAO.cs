using ForumAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Interfaces
{
    public interface PostDAO
    {
        Task<Post> GetPostById(int id);
        Task<Post> CreatePost(Post post);

        Task<ActionResult> AddPostToFavorites(int id, string username);

        Task<ActionResult> RemovePostFromFavorites(int id, string username);
        Task UpdatePostStatus(int id, bool newStatus);

        Task<List<Post>> SearchPostsByKeyword(string keyword);
    }
}
