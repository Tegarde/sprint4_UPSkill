﻿using Microsoft.AspNetCore.Mvc;
using ForumAPI.Services;
using ForumAPI.DTOs;
using ForumAPI.Mapper;
using ForumAPI.Interfaces;

using ForumAPI.CustomExceptions;
using ForumAPI.DTOs.PostDTOs;

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
        public async Task<ActionResult<List<PostDTO>>> GetPostsByKeyword(string keyword)
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


        [HttpPut("{id}")]
        public async Task<ActionResult<PostDTO>> UpdatePost(
            int id,
            [FromBody] UpdatePostDTO updatedPostDTO)
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
    }


}

