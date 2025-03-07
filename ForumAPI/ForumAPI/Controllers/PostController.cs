using Microsoft.AspNetCore.Mvc;
using ForumAPI.DTOs;
using ForumAPI.Mapper;
using ForumAPI.Interfaces;
using ForumAPI.CustomExceptions;
using ForumAPI.DTOs.PostDTOs;
using ForumAPI.Models;

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

        [HttpGet]
        public async Task<ActionResult<List<PostDTO>>> GetAllPosts()
        {
            try
            {
                var posts = await service.GetAllPosts();
                var postDTOs = posts.Select(p => PostMapper.ToDTO(p)).ToList();
                return Ok(postDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
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
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpGet("user/{username}")]
        public async Task<ActionResult<List<PostDTO>>> GetPostsByUser(string username)
        {
            try
            {
                var posts = await service.GetPostsByUser(username);
                var postDTOs = posts.Select(p => PostMapper.ToDTO(p)).ToList();
                return Ok(postDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpGet("sortBydate")]
        public ActionResult<List<PostDTO>> GetPostSortedByDate()
        {
            try
            {
                var posts = service.GetPostSortedByDate();
                var postDTOs = posts.Select(p => PostMapper.ToDTO(p)).ToList();
                return Ok(postDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpGet("top/{topN}")]
        public async Task<ActionResult<List<PostDTO>>> GetTopPostsByInteractions([FromRoute] int topN)
        {
            try
            {
                var posts = await service.GetTopPostsByInteractions(topN);
                var postDTOs = posts.Select(p => PostMapper.ToDTO(p)).ToList();
                return Ok(postDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpGet("favoritePosts")]
        public async Task<List<Post>> GetFavoritePosts([FromBody] string username)
        {
            return await service.GetFavoritePosts(username);
        }

        [HttpGet("between-dates")]
        public async Task<ActionResult<List<PostDTO>>> GetPostsBetweenDates(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var posts = await service.GetPostsBetweenDates(startDate, endDate);
                var postDTOs = posts.Select(p => PostMapper.ToDTO(p)).ToList();
                return Ok(postDTOs);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PostDTO>> UpdatePost(int id,[FromBody] UpdatePostDTO updatedPostDTO)
        {
            try
            {
                var updatedPost = PostMapper.FromUpdatePostDTO(updatedPostDTO);
                var post = await service.UpdatePost(id, updatedPost);
                return Ok(PostMapper.ToDTO(post));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new ResponseMessage { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Unable to update Post, something went wrong" });
            }
        }

        /// <summary>
        /// Creates a new post if the user exists.
        /// </summary>
        /// <param name="postDTO">The post details.</param>
        /// <returns>The created post.</returns>
        [HttpPost]
        public async Task<ActionResult> CreatePost([FromBody] CreatePostDTO postDTO)
        {
            try
            {
                var post = PostMapper.FromDTO(postDTO);
                var createdPost = await service.CreatePost(post);
                return CreatedAtAction(nameof(CreatePost), new ResponseMessage { Message = "Post created successfully." });
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


        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdatePostStatus(int id, [FromBody] bool newStatus)
        {
            try
            {
                await service.UpdatePostStatus(id, newStatus);
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

        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<List<PostDTO>>> SearchPostsByKeyword(string keyword)
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

        [HttpPost("favorite/add/{id}")]
        public async Task<ActionResult> AddPostToFavorites([FromRoute] int id, [FromBody] string username)
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

        [HttpPost("favorite/remove/{id}")]
        public async Task<ActionResult> RemovePostFromFavorites([FromRoute] int id, [FromBody] string username)
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

        /// <summary>
        /// Obtém o "hotness score" de um post específico.
        /// </summary>
        /// <param name="postId">ID do post</param>
        /// <returns>O score de popularidade do post</returns>
        [HttpGet("hotness/{postId}")]
        public async Task<IActionResult> GetPostHotness(int postId)
        {

            try
            {
                int hotnessScore = await service.GetPostHotnessScore(postId);

                return Ok(new { postId, hotnessScore });
            }
            catch(NotFoundException e)
            {
                return NotFound(new ResponseMessage { Message = e.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Obtém os posts mais "hot" (com mais interações).
        /// </summary>
        /// <param name="topN">Número de posts mais populares a retornar</param>
        /// <returns>Lista dos posts mais "hot"</returns>
        [HttpGet("hottest/{topN}")]
        public async Task<IActionResult> GetHottestPosts(int topN)
        {
            var posts = await service.GetHottestPosts(topN);
            return Ok(posts);
        }

        [HttpPatch("reset/{id}")]
        public async Task<ActionResult> ResetPost([FromRoute] int id)
        {
            try
            {
                await service.ResetPostInteractionCount(id);
                return Ok(new ResponseMessage { Message = "Post reset successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPost("like")]
        public async Task<ActionResult> LikePost([FromBody] PostLikeDTO likePostDTO)
        {
            try
            {
                await service.LikePost(InteractionsMapper.FromPostLikeDTO (likePostDTO));
                return Ok(new ResponseMessage { Message = "Post liked successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch(UserNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        [HttpDelete("like")]
        public async Task<ActionResult> UnLikePost([FromBody] PostLikeDTO likePostDTO)
        {
            try
            {
                await service.UnlikePost(InteractionsMapper.FromPostLikeDTO(likePostDTO));
                return Ok(new ResponseMessage { Message = "Post unliked successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        [HttpPost("dislike")]
        public async Task<ActionResult> DislikePost([FromBody] PostDislikeDTO postDislikeDTO)
        {
            try
            {
                await service.DislikePost(InteractionsMapper.FromPostDislikeDTO(postDislikeDTO));
                return Ok(new ResponseMessage { Message = "Post disliked successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        [HttpDelete("dislike")]
        public async Task<ActionResult> UnDislikePost([FromBody] PostDislikeDTO postDislikeDTO)
        {
            try
            {
                await service.UndislikePost(InteractionsMapper.FromPostDislikeDTO(postDislikeDTO));
                return Ok(new ResponseMessage { Message = "Post undisliked successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        [HttpGet("{id}/interactions/{username}")]
        public async Task<ActionResult> GetPostInteractionsByUser([FromRoute]int id, [FromRoute] string username)
        {
            try
            {
                int likes = await service.GetPostInteractionsByUser(id,username);
                return Ok(new { Interaction = likes });
            }
            catch(UserNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        [HttpGet("likesAndDislikes/{id}")] 
        public async Task<ActionResult> GetPostLikesAndDislikes([FromRoute] int id)
        {
            try
            {
                var likesAndDislikes = await service.GetLikesAndDislikesByPostId(id);
                return Ok(likesAndDislikes);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

    }
}


