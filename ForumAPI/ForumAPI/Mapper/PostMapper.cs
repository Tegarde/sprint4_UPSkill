using ForumAPI.DTOs;
using ForumAPI.Models;

namespace ForumAPI.Mapper
{
    /// <summary>
    /// Provides mapping methods to convert between Post and PostDTO.
    /// </summary>
    public static class PostMapper
    {
        /// <summary>
        /// Converts a Post entity to a PostDTO.
        /// </summary>
        /// <param name="post">The Post entity to convert.</param>
        /// <returns>A PostDTO representation of the Post.</returns>
        public static PostDTO ToDTO(Post post)
        {
            return new PostDTO
            {
                Title = post.Title,
                Content = post.Content,
                CreatedBy = post.CreatedBy,
                CreatedAt = post.CreatedAt,
                Status = post.Status,
                Category = post.Category,
                Interactions = post.Interactions
            };
        }

        /// <summary>
        /// Converts a PostDTO back to a Post entity.
        /// </summary>
        /// <param name="postDTO">The PostDTO to convert.</param>
        /// <returns>A Post entity representation of the PostDTO.</returns>
        public static Post FromDTO(PostDTO postDTO)
        {
            return new Post
            {
                Title = postDTO.Title,
                Content = postDTO.Content,
                CreatedBy = postDTO.CreatedBy,
                CreatedAt = postDTO.CreatedAt,
                Status = postDTO.Status,
                Category = postDTO.Category,
                Interactions = postDTO.Interactions
            };
        }
    }
}
