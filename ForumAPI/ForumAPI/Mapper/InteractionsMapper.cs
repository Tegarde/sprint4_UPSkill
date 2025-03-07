using ForumAPI.DTOs;
using ForumAPI.DTOs.CommentDTOs;
using ForumAPI.Models;

namespace ForumAPI.Mapper
{
    public class InteractionsMapper
    {
        public static PostLike FromPostLikeDTO(PostLikeDTO postLikeDTO)
        {
            return new PostLike
            {
                PostId = postLikeDTO.PostId,
                User = postLikeDTO.User,
                LikedAt = DateTime.UtcNow
            };
        }

        public static PostDislike FromPostDislikeDTO(PostDislikeDTO postDislikeDTO)
        {
            return new PostDislike
            {
                PostId = postDislikeDTO.PostId,
                User = postDislikeDTO.User,
                DislikedAt = DateTime.UtcNow
            };
        }

        public static CommentLike FromCommentLikeDTO(CommentLikeDTO commentLikeDTO)
        {
            return new CommentLike
            {
                CommentId = commentLikeDTO.CommentId,
                User = commentLikeDTO.User,
                LikedAt = DateTime.UtcNow
            };
        }

        

    }
}
