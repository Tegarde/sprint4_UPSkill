using Microsoft.AspNetCore.Mvc;
using ForumAPI.Services;
using ForumAPI.DTOs;
using ForumAPI.Mapper;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostService service;

        public PostController(PostService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Retrieves a post by its ID.
        /// </summary>
        /// <param name="id">The post ID.</param>
        /// <returns>The post details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPostById(int id)
        {
            try
            {
                var post = await service.GetPostById(id);
                return Ok(PostMapper.ToDTO(post));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new post if the user exists.
        /// </summary>
        /// <param name="postDTO">The post details.</param>
        /// <returns>The created post.</returns>
        [HttpPost]
        public async Task<ActionResult<PostDTO>> CreatePost([FromBody] PostDTO postDTO)
        {
            try
            {
                var post = PostMapper.FromDTO(postDTO);
                var createdPost = await service.CreatePost(post);
                return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, PostMapper.ToDTO(createdPost));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }
    }
}
