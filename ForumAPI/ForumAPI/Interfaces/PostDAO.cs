using ForumAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Interfaces
{
    public interface PostDAO
    {
        Task<List<Post>> GetAllPosts();

        Task<Post> GetPostById(int id);

        Task<List<Post>> GetPostsByUser(string username);

        List<Post> GetPostSortedByDate();

        Task<List<Post>> GetTopPostsByInteractions(int topN);

        Task<List<Post>> GetFavoritePosts(string username);
        Task<List<Post>> GetPostsBetweenDates(DateTime startDate, DateTime endDate);

        Task<Post> CreatePost(Post post);

        Task<Post> UpdatePost(int postId, Post updatedPost);

        Task<ActionResult> AddPostToFavorites(int postId, string username);

        Task<ActionResult> RemovePostFromFavorites(int postId, string username);

        Task UpdatePostStatus(int id, bool newStatus);

        Task<List<Post>> SearchPostsByKeyword(string keyword);
    }
}
