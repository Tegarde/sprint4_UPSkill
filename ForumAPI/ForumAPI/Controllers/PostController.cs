using Microsoft.AspNetCore.Mvc;
using ForumAPI.Services;
using ForumAPI.DTOs;
using ForumAPI.Mapper;
using ForumAPI.Interfaces;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostDAO service;

        public PostController(PostDAO service)
        {
            this.service = service;
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="id">The post ID.</param>
        /// <returns>The post details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CreatePostDTO>> GetPostById(int id)
        {
            try
            {
                var post = await service.GetPostById(id);
                return Ok(PostMapper.ToDTO(post));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new post if the user exists.
        /// </summary>
        /// <param name="postDTO">The post details.</param>
        /// <returns>The created post.</returns>
        [HttpPost]
        public async Task<ActionResult<CreatePostDTO>> CreatePost([FromBody] CreatePostDTO postDTO)
        {
            try
            {
                var post = PostMapper.FromDTO(postDTO);
                var createdPost = await service.CreatePost(post);
                return CreatedAtAction(nameof(CreatePost), new ResponseMessage { Message = "Post created successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPost("{id}/favorite")]
        public async Task<ActionResult> AddPostToFavorites(int id, [FromHeader] string username)
        {
            try
            {
                await service.AddPostToFavorites(id, username);
                return Ok(new ResponseMessage { Message = "Post added to favorites successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPost("{id}/favorite")]
        public async Task<ActionResult> RemovePostFromFavorites(int id, [FromHeader] string username)
        {
            try
            {
                await service.RemovePostFromFavorites(id, username);
                return Ok(new ResponseMessage { Message = "Post removed from favorites successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpGet("status-options")]
        public ActionResult<IEnumerable<string>> GetPostStatusOptions()
        {
            var statusOptions = new List<string> { "Active", "Inactive", "Archived" };
            return Ok(statusOptions);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdatePostStatus(int id, [FromBody] bool newStatus, [FromHeader] string userRole)
        {
            try
            {
                await service.UpdatePostStatus(id, newStatus, userRole);
                return Ok(new ResponseMessage { Message = "Post status updated successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ResponseMessage { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpGet("/search/{keyword}")]
        public async Task<ActionResult<List<CreatePostDTO>>> GetPostsByKeyword(string keyword)
        {
            try
            {
                var posts = await service.SearchPostsByKeyword(keyword);
                if (!posts.Any())
                {
                    return NoContent();
                }
                return Ok(posts.Select(post => PostMapper.ToDTO(post)).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }
    }
}
