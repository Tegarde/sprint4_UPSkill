using ForumAPI.CustomExceptions;
using ForumAPI.DTOs;
using ForumAPI.DTOs.CommentDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Mapper;
using ForumAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentDAO service;

        public CommentsController(CommentDAO service)
        {
            this.service = service;
        }


        [HttpPost]
        public async Task<ActionResult> CommentAComment([FromBody] CommentACommentDTO commentDTO)
        {
            try
            {
                Comment comment = await service.CommentAComment(CommentMapper.FromCommentACommentDTO(commentDTO));
                return CreatedAtAction(nameof(CommentAComment), new ResponseMessage{ Message = "Comment successfully created" });
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

        [HttpPost("event/comment")]
        public async Task<ActionResult> CommentAnEvent([FromBody] CommentAnEventDTO commentDTO)
        {
            try 
            {
                Comment comment = await service.CommentAnEvent(CommentMapper.FromCommentAnEventDTO(commentDTO));
                return CreatedAtAction(nameof(CommentAnEvent), new ResponseMessage{ Message = "Comment successfully created" });
            }
            catch (NotFoundException ex)
            { 
                return StatusCode(404, new ResponseMessage { Message = ex.Message });   
            }
            catch (UserNotFoundException ex)
            {
                return StatusCode(404, new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = ex.Message });
            }
        }

        [HttpPost("post/comment")]
        public async Task<ActionResult> CommentAPost([FromBody]CommentAPostDTO commentDTO) 
        {
        try
            {
                Comment comment = await service.CommentAPost(CommentMapper.FromCommentAPostDTO(commentDTO));
                return CreatedAtAction(nameof(CommentAPost), new ResponseMessage { Message = "Comment successfully created" });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch(UserNotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
        }


    }
}
