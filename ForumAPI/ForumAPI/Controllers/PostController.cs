using Microsoft.AspNetCore.Mvc;
using ForumAPI.DTOs;
using ForumAPI.Mapper;
using ForumAPI.Interfaces;
using ForumAPI.CustomExceptions;
using ForumAPI.DTOs.PostDTOs;
using ForumAPI.Services;
using ForumAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ForumAPI.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostDAO service;
        private readonly FileUploadService fileUploadService;

        public PostController(PostDAO service, FileUploadService fileUploadService)
        {
            this.service = service;
            this.fileUploadService = fileUploadService;
        }

        /// <summary>
        /// Retrieves all posts.
        /// </summary>
        /// <returns>A list of all posts</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Get all posts", Description = "Fetches a list of all posts.")]
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
        [ProducesResponseType(typeof(PostDTO), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Get post by ID", Description = "Fetches a specific post using its ID.")]
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

        /// <summary>
        /// Retrieves posts by a specific user.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>A list of posts by the user</returns>
        [HttpGet("user/{username}")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Get posts by user", Description = "Fetch posts created by a specific user.")]
        public async Task<ActionResult<List<PostDTO>>> GetPostsByUser(string username)
        {
            try
            {
                var posts = await service.GetPostsByUser(username);
                var postDTOs = posts.Select(p => PostMapper.ToDTO(p)).ToList();
                return Ok(postDTOs);
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
        /// Retrieves posts sorted by date.
        /// </summary>
        /// <returns>A list of posts sorted by date</returns>
        [HttpGet("sortBydate")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Get posts sorted by date", Description = "Fetches a list of posts sorted by date.")]
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

        /// <summary>
        /// Retrieves top posts by interactions.
        /// </summary>
        /// <param name="topN">The number of top posts to retrieve</param>
        /// <returns>A list of top posts</returns>
        [HttpGet("top/{topN}")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Get top posts by interactions", Description = "Fetch the top posts by user interactions.")]
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

        /// <summary>
        /// Retrieves favorite posts of a user.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <returns>A list of favorite posts</returns>
        [HttpGet("favoritePosts/{username}")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Get favorite posts of a user", Description = "Fetch the favorite posts of a specific user.")]
        public async Task<ActionResult> GetFavoritePosts(string username)
        {
            try
            {
                var posts = await service.GetFavoritePosts(username);
                var postDTOs = posts.Select(p => PostMapper.ToDTO(p)).ToList();
                return Ok(postDTOs);
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Retrieves posts between two dates.
        /// </summary>
        /// <param name="startDate">The start date for filtering posts</param>
        /// <param name="endDate">The end date for filtering posts</param>
        /// <returns>A list of posts created between the specified dates</returns>
        [HttpGet("between-dates")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Get posts between two dates", Description = "Fetch posts created between the given start and end date.")]
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

        /// <summary>
        /// Updates the details of an existing post.
        /// </summary>
        /// <param name="id">The ID of the post to update</param>
        /// <param name="updatedPostDTO">The updated post details</param>
        /// <returns>The updated post</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PostDTO), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        [SwaggerOperation(Summary = "Update a post", Description = "Update the details of an existing post.")]
        public async Task<ActionResult<PostDTO>> UpdatePost(int id, [FromBody] UpdatePostDTO updatedPostDTO)
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
        /// Creates a new post.
        /// </summary>
        /// <param name="postDTO">The post details</param>
        /// <returns>The created post</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PostDTO), 201)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Create a new post", Description = "Create a new post if the user exists.")]
        public async Task<ActionResult> CreatePost(CreatePostDTO postDTO)
        {
            try
            {
                var post = PostMapper.FromDTO(postDTO);
                if (postDTO.Image != null)
                {
                    post.Image = await fileUploadService.UploadFileAsync(postDTO.Image);
                }
                var createdPost = await service.CreatePost(post);
                return CreatedAtAction(nameof(CreatePost), new { id = createdPost.Id }, PostMapper.ToDTO(createdPost));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
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


        [HttpPut("{id}/status/{newStatus}")]
        public async Task<ActionResult> UpdatePostStatus(int id, bool newStatus)
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

        /// <summary>
        /// Searches posts by a specific keyword.
        /// </summary>
        /// <param name="keyword">The keyword to search posts by</param>
        /// <returns>A list of posts matching the keyword</returns>
        [HttpGet("search/{keyword}")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Search posts by keyword", Description = "Searches posts using a specific keyword.")]
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

        /// <summary>
        /// Adds a post to favorites.
        /// </summary>
        /// <param name="dto">The post favorite details</param>
        /// <returns>A success message</returns>
        [HttpPost("favorite")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [SwaggerOperation(Summary = "Add post to favorites", Description = "Adds a specified post to the favorites of a user.")]
        public async Task<ActionResult> AddPostToFavorites(PostFavoriteDTO dto)
        {
            try
            {
                await service.AddPostToFavorites(dto.PostId, dto.User);
                return Ok(new ResponseMessage { Message = "Post added to favorites successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
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
        /// Remove a post from a user's favorites.
        /// </summary>
        /// <param name="dto">Data transfer object containing PostId and User information.</param>
        /// <returns>Response indicating if the post was successfully removed from favorites.</returns>
        [HttpDelete("favorite")]
        [SwaggerOperation(Summary = "Remove a post from a user's favorites")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        public async Task<ActionResult> RemovePostFromFavorites(PostFavoriteDTO dto)
        {
            try
            {
                await service.RemovePostFromFavorites(dto.PostId, dto.User);
                return Ok(new ResponseMessage { Message = "Post removed from favorites successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
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
        /// Get the "hotness score" of a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <returns>The "hotness score" of the post.</returns>
        [HttpGet("hotness/{postId}")]
        [SwaggerOperation(Summary = "Get the hotness score of a post")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        public async Task<IActionResult> GetPostHotness(int postId)
        {
            try
            {
                int hotnessScore = await service.GetPostHotnessScore(postId);
                return Ok(new { postId, hotnessScore });
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

        /// <summary>
        /// Get the hottest posts based on interactions.
        /// </summary>
        /// <param name="topN">Number of top "hot" posts to return.</param>
        /// <returns>A list of the hottest posts.</returns>
        [HttpGet("hottest/{topN}")]
        [SwaggerOperation(Summary = "Get the top hottest posts")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 204)]
        public async Task<IActionResult> GetHottestPosts(int topN)
        {
            try
            {
                var posts = await service.GetHottestPosts(topN);
                return (posts.Any()) ? Ok(posts.Select(post => PostMapper.ToDTO(post)).ToList()) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Get the hottest posts from the last month.
        /// </summary>
        /// <param name="topN">Number of top posts to return.</param>
        /// <returns>A list of the hottest posts from the last month.</returns>
        [HttpGet("monthly/{topN}")]
        [SwaggerOperation(Summary = "Get the hottest posts from the last month")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 204)]
        public async Task<IActionResult> GetMonthlyPosts(int topN)
        {
            try
            {
                var posts = await service.GetHottestPostsFromLastMonth(topN);
                return (posts.Any()) ? Ok(posts.Select(post => PostMapper.ToDTO(post)).ToList()) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Get the hottest posts from the last day.
        /// </summary>
        /// <param name="topN">Number of top posts to return.</param>
        /// <returns>A list of the hottest posts from the last day.</returns>
        [HttpGet("daily/{topN}")]
        [SwaggerOperation(Summary = "Get the hottest posts from the last day")]
        [ProducesResponseType(typeof(List<PostDTO>), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 204)]
        public async Task<IActionResult> GetDailyPosts(int topN)
        {
            try
            {
                var posts = await service.GetHottestPostsFromLastDay(topN);
                return (posts.Any()) ? Ok(posts.Select(post => PostMapper.ToDTO(post)).ToList()) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Reset the interaction count for a specific post.
        /// </summary>
        /// <param name="id">The ID of the post to reset.</param>
        /// <returns>A message indicating the reset status.</returns>
        [HttpPatch("reset/{id}")]
        [SwaggerOperation(Summary = "Reset the interaction count of a post")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
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

        /// <summary>
        /// Like a post.
        /// </summary>
        /// <param name="likePostDTO">Data transfer object containing information about the post and user liking it.</param>
        /// <returns>Response indicating if the post was liked successfully.</returns>
        [HttpPost("like")]
        [SwaggerOperation(Summary = "Like a post")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        [ProducesResponseType(typeof(ResponseMessage), 409)] // For conflict if user already liked it
        public async Task<ActionResult> LikePost([FromBody] PostLikeDTO likePostDTO)
        {
            try
            {
                await service.LikePost(InteractionsMapper.FromPostLikeDTO(likePostDTO));
                return Ok(new ResponseMessage { Message = "Post liked successfully." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
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

        /// <summary>
        /// Remove a like from a post.
        /// </summary>
        /// <param name="likePostDTO">The data transfer object containing PostId and User information.</param>
        /// <returns>Response indicating if the post was unliked successfully.</returns>
        [HttpDelete("like")]
        [SwaggerOperation(Summary = "Remove a like from a post")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
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
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Dislike a post.
        /// </summary>
        /// <param name="postDislikeDTO">The data transfer object containing PostId and User information.</param>
        /// <returns>Response indicating if the post was disliked successfully.</returns>
        [HttpPost("dislike")]
        [SwaggerOperation(Summary = "Dislike a post")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
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
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Remove a dislike from a post.
        /// </summary>
        /// <param name="postDislikeDTO">The data transfer object containing PostId and User information.</param>
        /// <returns>Response indicating if the post was undisliked successfully.</returns>
        [HttpDelete("dislike")]
        [SwaggerOperation(Summary = "Remove a dislike from a post")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
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
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Get the interactions (likes/dislikes) of a post by a specific user.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <param name="username">The username of the user interacting with the post.</param>
        /// <returns>The interaction count for the post by the specific user.</returns>
        [HttpGet("{id}/interactions/{username}")]
        [SwaggerOperation(Summary = "Get post interactions by user")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        public async Task<ActionResult> GetPostInteractionsByUser([FromRoute] int id, [FromRoute] string username)
        {
            try
            {
                int likes = await service.GetPostInteractionsByUser(id, username);
                return Ok(new { Interaction = likes });
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
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

        /// <summary>
        /// Get the likes and dislikes count for a specific post.
        /// </summary>
        /// <param name="id">The ID of the post.</param>
        /// <returns>The likes and dislikes count for the post.</returns>
        [HttpGet("likesAndDislikes/{id}")]
        [SwaggerOperation(Summary = "Get post likes and dislikes count")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
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

        /// <summary>
        /// Check if a post is a favorite for a specific user.
        /// </summary>
        /// <param name="postId">The ID of the post.</param>
        /// <param name="username">The username of the user.</param>
        /// <returns>The favorite status of the post for the user.</returns>
        [HttpGet("{postId}/favorite/{username}")]
        [SwaggerOperation(Summary = "Check if a post is a favorite for a specific user")]
        [ProducesResponseType(typeof(ResponseMessage), 200)]
        [ProducesResponseType(typeof(ResponseMessage), 404)]
        [ProducesResponseType(typeof(ResponseMessage), 400)]
        public async Task<ActionResult> IsPostFavorite([FromRoute] int postId, [FromRoute] string username)
        {
            try
            {
                int favorite = await service.GetPostFavoriteByUsername(new PostFavorite { PostId = postId, User = username });
                return Ok(new { Favorite = favorite });
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
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }
    }
}
