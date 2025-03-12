
using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.DTOs.PostDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Mapper;
using ForumAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreenitorController : ControllerBase
    {
        private readonly GreenitorDAO service;
        private readonly PostDAO postService;
        private readonly CommentDAO commentService;
        private readonly EventDAO eventService;
        private readonly FileUploadService fileUploadService;

        public GreenitorController(GreenitorDAO service, PostDAO postService, CommentDAO commentService, EventDAO eventService, FileUploadService fileUploadService)
        {
            this.service = service;
            this.postService = postService;
            this.commentService = commentService;
            this.eventService = eventService;
            this.fileUploadService = fileUploadService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage>> RegisterUser([FromForm] RegisterUserDTO greenitor)
        {
            try
            {   
                RegisterUserWithImageDTO user = GreenitorMapper.toRegisterUserWithImageDTO(greenitor);

                if (greenitor.Image!= null) 
                { 
                    user.Image = await fileUploadService.UploadFileAsync(greenitor.Image);
                }
                else
                {
                    user.Image = "uploads/f83f0f0a-d7ed-40f4-901b-d68b3b431879.jpg";
                }
                    ResponseMessage message = await service.RegisterUser(user);
                return CreatedAtAction(nameof(RegisterUser), message);

            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                TokenDTO token = await service.Login(loginDTO);
                return Ok(token);
            }
            catch(ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<GreenitorWithoutRoleDTO>> GetUserByUsername(string username)
        {
            try
            {
                var greenitor = await service.GetUserByUsername(username);

                return Ok(GreenitorMapper.toGreenitorWithoutRoleDTO(greenitor));
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("notifications/{username}")]
        public async Task<ActionResult<List<PostNotificationDTO>>> GetNotifications(string username)
        {
            try
            {
                var posts = await postService.GetNotificationsByUser(username);
                return (posts.Any()) ? Ok(posts.Select(post => PostMapper.ToPostNotificationDTO(post)).ToList()) : NoContent();
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpGet("stats/{username}")] 
        public async Task<ActionResult<GreenitorStatisticsDTO>> GetGreenitorStats(string username)
        {
            try
            {
                var postStats = await postService.GetPostStatisticsByUsername(username);

                int eventStats = await eventService.GetEventStatisticsByUsername(username);

                postStats.EventAttendances = eventStats;

                var commentStats = await commentService.GetCommentStatisticsByUsername(username);

                postStats.Comments = commentStats.Comments;
                postStats.LikesInComments = commentStats.LikesInComments;

                return Ok(postStats);
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }


        [HttpGet("all")]
        public async Task<ActionResult<List<GreenitorWithoutRoleDTO>>> GetAllGreenitors()
        {
            try
            {
                return Ok( await service.GetAllGreenitors());
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage{ Message = "Something went wrong" });
            }
        }
    }
}
