using ForumAPI.Interfaces;

namespace ForumAPI.Services
{
    public class PostService : PostDAO
    {
        private static HttpClient client;

        private static readonly string BaseUrl = "http://localhost:8080/api/post";

        public PostService()
        {
            client = new HttpClient();
        }

        //public async Task<Post> CreatePost(Post post)
        //{
        //    var = await client.
        //}
    }
}
