using ForumAPI.DTOs;
using ForumAPI.DTOs.CommentDTOs;
using ForumAPI.Models;

namespace ForumAPI.Mapper
{
    /// <summary>
    /// Provides methods to map interaction-related DTOs (PostLikeDTO, PostDislikeDTO, CommentLikeDTO) to their respective model entities (PostLike, PostDislike, CommentLike).
    /// </summary>
    public class InteractionsMapper
    {
        /// <summary>
        /// Maps a PostLikeDTO to a PostLike entity.
        /// </summary>
        /// <param name="postLikeDTO">The PostLikeDTO to be mapped.</param>
        /// <returns>A PostLike entity populated with data from the provided PostLikeDTO.</returns>
        public static PostLike FromPostLikeDTO(PostLikeDTO postLikeDTO)
        {
            return new PostLike
            {
                PostId = postLikeDTO.PostId,
                User = postLikeDTO.User,
                LikedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Maps a PostDislikeDTO to a PostDislike entity.
        /// </summary>
        /// <param name="postDislikeDTO">The PostDislikeDTO to be mapped.</param>
        /// <returns>A PostDislike entity populated with data from the provided PostDislikeDTO.</returns>
        public static PostDislike FromPostDislikeDTO(PostDislikeDTO postDislikeDTO)
        {
            return new PostDislike
            {
                PostId = postDislikeDTO.PostId,
                User = postDislikeDTO.User,
                DislikedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Maps a CommentLikeDTO to a CommentLike entity.
        /// </summary>
        /// <param name="commentLikeDTO">The CommentLikeDTO to be mapped.</param>
        /// <returns>A CommentLike entity populated with data from the provided CommentLikeDTO.</returns>
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