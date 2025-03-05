using ForumAPI.CustomExceptions;
using ForumAPI.DTOs;
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
        public ActionResult CommentAComment([FromBody] CommentACommentDTO commentDTO)
        {
            try
            {
                Comment comment = service.CommentAComment(CommentMapper.FromCommentACommentDTO(commentDTO));
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

        [HttpPost("event/{eventId}")]
        public async Task<ActionResult> CommentAnEvent([FromRoute] int eventId, [FromBody] CommentAnEventDTO commentDTO)
        {
            try 
            {
                Comment comment = service.CommentAnEvent(CommentMapper.FromCommentAnEventDTO(commentDTO), eventId);
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
    }
}
