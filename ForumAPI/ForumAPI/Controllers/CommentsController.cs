using ForumAPI.CustomExceptions;
using ForumAPI.DTOs;
using ForumAPI.DTOs.CommentDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Mapper;
using ForumAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ForumAPI.Controllers
{
    /// <summary>
    /// Controller for managing comments in the forum.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Comment Management")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentDAO service;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="service">The comment service.</param>
        public CommentsController(CommentDAO service)
        {
            this.service = service;
        }

        /// <summary>
        /// Allows a user to comment on a comment.
        /// </summary>
        /// <param name="commentDTO">Comment data transfer object containing the comment details.</param>
        /// <returns>A response message confirming the comment creation.</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Allows a user to comment on a comment.",
            Description = "This endpoint enables a user to comment on an existing comment."
        )]
        [SwaggerResponse(201, "Comment successfully created.")]
        [SwaggerResponse(400, "Something went wrong.")]
        [SwaggerResponse(404, "Comment not found.")]
        public async Task<ActionResult> CommentAComment([FromBody] CommentACommentDTO commentDTO)
        {
            try
            {
                Comment comment = await service.CommentAComment(CommentMapper.FromCommentACommentDTO(commentDTO));
                return CreatedAtAction(nameof(CommentAComment), new ResponseMessage { Message = "Comment successfully created" });
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404, new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Allows a user to comment on an event.
        /// </summary>
        /// <param name="commentDTO">Comment data transfer object containing the comment details.</param>
        /// <returns>A response message confirming the comment creation.</returns>
        [HttpPost("event/comment")]
        [SwaggerOperation(
            Summary = "Allows a user to comment on an event.",
            Description = "This endpoint enables a user to comment on an event."
        )]
        [SwaggerResponse(201, "Comment successfully created.")]
        [SwaggerResponse(404, "Event not found.")]
        [SwaggerResponse(400, "Something went wrong.")]
        public async Task<ActionResult> CommentAnEvent([FromBody] CommentAnEventDTO commentDTO)
        {
            try
            {
                Comment comment = await service.CommentAnEvent(CommentMapper.FromCommentAnEventDTO(commentDTO));
                return CreatedAtAction(nameof(CommentAnEvent), new ResponseMessage { Message = "Comment successfully created" });
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404, new ResponseMessage { Message = ex.Message });
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Allows a user to comment on a post.
        /// </summary>
        /// <param name="commentDTO">Comment data transfer object containing the comment details.</param>
        /// <returns>A response message confirming the comment creation.</returns>
        [HttpPost("post/comment")]
        [SwaggerOperation(
            Summary = "Allows a user to comment on a post.",
            Description = "This endpoint enables a user to comment on a post."
        )]
        [SwaggerResponse(201, "Comment successfully created.")]
        [SwaggerResponse(404, "Post not found.")]
        [SwaggerResponse(400, "Something went wrong.")]
        public async Task<ActionResult> CommentAPost([FromBody] CommentAPostDTO commentDTO)
        {
            try
            {
                Comment comment = await service.CommentAPost(CommentMapper.FromCommentAPostDTO(commentDTO));
                return CreatedAtAction(nameof(CommentAPost), new ResponseMessage { Message = "Comment successfully created" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Allows a user to like a comment.
        /// </summary>
        /// <param name="likeDTO">Comment like data transfer object.</param>
        /// <returns>A response message confirming the comment like action.</returns>
        [HttpPost("comment/like/")]
        [SwaggerOperation(
            Summary = "Allows a user to like a comment.",
            Description = "This endpoint enables a user to like a comment."
        )]
        [SwaggerResponse(200, "Comment liked successfully.")]
        [SwaggerResponse(404, "Comment not found.")]
        [SwaggerResponse(400, "Invalid input.")]
        public async Task<ActionResult> LikeComment([FromBody] CommentLikeDTO likeDTO)
        {
            try
            {
                CommentLike commentLike = await service.LikeComment(InteractionsMapper.FromCommentLikeDTO(likeDTO));
                return Ok(new ResponseMessage { Message = "Comment liked successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Allows a user to unlike a comment.
        /// </summary>
        /// <param name="likeDTO">Comment like data transfer object.</param>
        /// <returns>A response message confirming the comment unliking action.</returns>
        [HttpDelete("comment/like/")]
        [SwaggerOperation(
            Summary = "Allows a user to unlike a comment.",
            Description = "This endpoint enables a user to unlike a comment."
        )]
        [SwaggerResponse(200, "Comment unliked successfully.")]
        [SwaggerResponse(404, "Comment not found.")]
        [SwaggerResponse(400, "Invalid input.")]
        public async Task<ActionResult> UnLikeComment([FromBody] CommentLikeDTO likeDTO)
        {
            try
            {
                CommentLike commentLike = await service.UnLikeComment(InteractionsMapper.FromCommentLikeDTO(likeDTO));
                return Ok(new ResponseMessage { Message = "Comment unliked successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Get the count of likes for a comment.
        /// </summary>
        /// <param name="commentId">ID of the comment.</param>
        /// <returns>The like count for the specified comment.</returns>
        [HttpGet("likesCount/{commentId}")]
        [SwaggerOperation(
            Summary = "Get the like count for a specific comment.",
            Description = "This endpoint returns the number of likes for a specific comment."
        )]
        [SwaggerResponse(200, "Number of likes retrieved successfully.")]
        [SwaggerResponse(404, "Comment not found.")]
        [SwaggerResponse(400, "Something went wrong.")]
        public async Task<ActionResult> GetLikesCount(int commentId)
        {
            try
            {
                int likesCount = await service.GetNumberOfLikesFromCommentId(commentId);
                return Ok(new { CommentId = commentId, LikesCount = likesCount });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Get comment interactions (likes) by a user for a specific comment.
        /// </summary>
        /// <param name="id">ID of the comment.</param>
        /// <param name="username">Username of the user.</param>
        /// <returns>The number of interactions (likes) by the user for the specified comment.</returns>
        [HttpGet("{id}/likes/{username}")]
        [SwaggerOperation(
            Summary = "Get comment interactions (likes) by a user.",
            Description = "This endpoint returns the number of interactions (likes) a user has for a specific comment."
        )]
        [SwaggerResponse(200, "Interactions retrieved successfully.")]
        [SwaggerResponse(404, "Comment or user not found.")]
        [SwaggerResponse(400, "Something went wrong.")]
        public async Task<ActionResult<int>> GetCommentInteractionsByUser(int id, string username)
        {
            try
            {
                int likes = await service.GetCommentInteractionsByUser(id, username);
                return Ok(likes);
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Get all comments for a specific comment.
        /// </summary>
        /// <param name="parentCommentId">ID of the parent comment.</param>
        /// <returns>A list of comments related to the specified parent comment.</returns>
        [HttpGet("{parentCommentId}/comments")]
        [SwaggerOperation(
            Summary = "Get all comments for a specific comment.",
            Description = "This endpoint returns a list of comments related to the specified parent comment."
        )]
        [SwaggerResponse(200, "Comments retrieved successfully.")]
        [SwaggerResponse(404, "Parent comment not found.")]
        [SwaggerResponse(400, "Something went wrong.")]
        public async Task<ActionResult<List<CommentFromCommentDTO>>> GetCommentsToComment(int parentCommentId)
        {
            try
            {
                List<Comment> comments = await service.GetCommentsByCommentId(parentCommentId);
                return Ok(comments.Select(CommentMapper.ToCommentFromCommentDTO).ToList());
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "Something went wrong" });
            }
        }
    }
}