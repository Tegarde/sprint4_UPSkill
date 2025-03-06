using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    public interface PostDAO
    {
        Task<List<Post>> GetAllPosts();

        Task<Post> GetPostById(int id);

        Task<List<Post>> GetPostsByUser(string username);

        List<Post> GetPostSortedByDate();

        Task<List<Post>> GetTopPostsByInteractions(int topN);

        Task<List<Post>> GetPostsBetweenDates(DateTime startDate, DateTime endDate);

        Task<Post> CreatePost(Post post);

        Task<Post> UpdatePost(int postId, Post updatedPost);

        Task UpdatePostStatus(int id, bool newStatus);

        Task<List<Post>> SearchPostsByKeyword(string keyword);
    }
}
