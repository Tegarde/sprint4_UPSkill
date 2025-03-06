using ForumAPI.DTOs;
using ForumAPI.DTOs.PostDTOs;
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
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedBy = post.CreatedBy,
                CreatedAt = post.CreatedAt,
                Status = post.Status,
                Category = post.Category,
                Interactions = post.Interactions,
                LikedBy = post.LikedBy.Count,
                DislikedBy = post.DislikedBy.Count,
                Comments = post.Comments
                    .Select(CommentMapper.ToDTO)
                    .ToList()
            };
        }

        /// <summary>
        /// Converts a PostDTO back to a Post entity.
        /// </summary>
        /// <param name="postDTO">The PostDTO to convert.</param>
        /// <returns>A Post entity representation of the PostDTO.</returns>
        public static Post FromDTO(CreatePostDTO createPostDTO)
        {
            return new Post
            {
                Title = createPostDTO.Title,
                Content = createPostDTO.Content,
                CreatedBy = createPostDTO.CreatedBy,
                Category = createPostDTO.Category
            };
        }

        public static Post FromUpdatePostDTO(UpdatePostDTO updatePostDTO)
        {
            return new Post
            {
                Id = updatePostDTO.Id,
                Title = updatePostDTO.Title,
                Content = updatePostDTO.Content,
                CreatedBy = updatePostDTO.CreatedBy,

            };
        }
    }
}
