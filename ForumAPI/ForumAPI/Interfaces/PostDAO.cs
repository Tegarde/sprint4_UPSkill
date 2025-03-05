using ForumAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Interfaces
{
    public interface PostDAO
    {

        Task<Post> GetPostById(int id);
        Task<Post> CreatePost(Post post);

        Task<ActionResult> AddPostToFavorites(int id, [FromHeader] string username);

        Task<ActionResult> RemovePostFromFavorites(int id, [FromHeader] string username);
        Task UpdatePostStatus(int id, bool newStatus, string userRole);

        Task<List<Post>> SearchPostsByKeyword(string keyword);


        Task<List<Post>> GetPostsByUser(string username);

        Task<List<Post>> GetAllPosts();

        Task<Post> UpdatePost(int postId, Post updatedPost);

        List<Post> GetPostSortedByDate();

        Task<List<Post>> GetTopPostsByInteractions(int topN);

        Task<List<Post>> GetPostsBetweenDates(DateTime startDate, DateTime endDate);

    }
}
