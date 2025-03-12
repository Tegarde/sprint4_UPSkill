using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.DTOs.PostDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Mapper;
using ForumAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ForumAPI.Controllers
{
    /// <summary>
    /// Controller for managing Greenitor (user) operations such as registration, login, retrieving user data, notifications, and statistics.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GreenitorController : ControllerBase
    {
        private readonly GreenitorDAO service;
        private readonly PostDAO postService;
        private readonly CommentDAO commentService;
        private readonly EventDAO eventService;
        private readonly FileUploadService fileUploadService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GreenitorController"/> class.
        /// </summary>
        /// <param name="service">Service for user-related operations.</param>
        /// <param name="postService">Service for handling posts.</param>
        /// <param name="commentService">Service for managing comments.</param>
        /// <param name="eventService">Service for tracking event participation.</param>
        /// <param name="fileUploadService">Service for handling file uploads.</param>
        public GreenitorController(GreenitorDAO service, PostDAO postService, CommentDAO commentService, EventDAO eventService, FileUploadService fileUploadService)
        {
            this.service = service;
            this.postService = postService;
            this.commentService = commentService;
            this.eventService = eventService;
            this.fileUploadService = fileUploadService;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="greenitor">The data of the user to register.</param>
        /// <remarks>
        /// This endpoint registers a new Greenitor (user) by receiving the user's data and optional profile image.
        /// </remarks>
        /// <returns>A response message confirming successful registration or an error message.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Register a new Greenitor", Description = "Registers a new user in the system.")]
        [SwaggerResponse(201, "User registered successfully.")]
        [SwaggerResponse(400, "Bad request. Something went wrong during registration.")]
        public async Task<ActionResult<ResponseMessage>> RegisterUser(RegisterUserDTO greenitor)
        {
            try
            {
                RegisterUserWithImageDTO user = GreenitorMapper.toRegisterUserWithImageDTO(greenitor);

                if (greenitor.Image != null)
                {
                    user.Image = await fileUploadService.UploadFileAsync(greenitor.Image);
                }
                else
                {
                    user.Image = "uploads/f83f0f0a-d7ed-40f4-901b-d68b3b431879.jpg";  // default image path
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
                return StatusCode(400, "Something went wrong");
            }
        }

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <param name="loginDTO">The credentials of the user trying to log in.</param>
        /// <remarks>
        /// This endpoint allows a registered Greenitor to log in and receive a token for authentication.
        /// </remarks>
        /// <returns>A token if login is successful.</returns>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Log in a user", Description = "Logs in an existing user and provides a token.")]
        [SwaggerResponse(200, "Successfully logged in and received a token.")]
        [SwaggerResponse(400, "Bad request. Invalid credentials.")]
        public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                TokenDTO token = await service.Login(loginDTO);
                return Ok(token);
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

        /// <summary>
        /// Retrieves a user’s details by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <remarks>
        /// This endpoint fetches the details of a Greenitor (user) excluding their role information.
        /// </remarks>
        /// <returns>The details of the user.</returns>
        [HttpGet("username/{username}")]
        [SwaggerOperation(Summary = "Get user details by username", Description = "Fetches a user's details based on their username.")]
        [SwaggerResponse(200, "Successfully fetched the user details.")]
        [SwaggerResponse(400, "Bad request. User not found.")]
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

        /// <summary>
        /// Retrieves notifications for a specific user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <remarks>
        /// This endpoint fetches a list of posts or notifications that require the user's attention.
        /// </remarks>
        /// <returns>A list of notifications for the user.</returns>
        [HttpGet("notifications/{username}")]
        [SwaggerOperation(Summary = "Get notifications for a user", Description = "Fetches notifications or posts requiring a user’s attention.")]
        [SwaggerResponse(200, "Successfully fetched the notifications.")]
        [SwaggerResponse(204, "No notifications found.")]
        [SwaggerResponse(400, "Bad request. Error while fetching notifications.")]
        public async Task<ActionResult<List<PostNotificationDTO>>> GetNotifications(string username)
        {
            try
            {
                var posts = await postService.GetNotificationsByUser(username);
                return posts.Any() ? Ok(posts.Select(post => PostMapper.ToPostNotificationDTO(post)).ToList()) : NoContent();
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

        /// <summary>
        /// Retrieves statistics for a specific user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <remarks>
        /// This endpoint fetches a user's post statistics, event attendances, and comment statistics.
        /// </remarks>
        /// <returns>The statistics of the user.</returns>
        [HttpGet("stats/{username}")]
        [SwaggerOperation(Summary = "Get user statistics", Description = "Fetches user statistics including posts, events, and comments.")]
        [SwaggerResponse(200, "Successfully retrieved the user statistics.")]
        [SwaggerResponse(400, "Bad request. Error while fetching statistics.")]
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

        /// <summary>
        /// Retrieves a list of all Greenitors (users).
        /// </summary>
        /// <remarks>
        /// This endpoint returns a list of all users excluding their role information.
        /// </remarks>
        /// <returns>A list of all users in the system.</returns>
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all Greenitors", Description = "Fetches a list of all Greenitors in the system.")]
        [SwaggerResponse(200, "Successfully fetched the list of Greenitors.")]
        [SwaggerResponse(400, "Bad request. Error while fetching the list.")]
        public async Task<ActionResult<List<GreenitorWithoutRoleDTO>>> GetAllGreenitors()
        {
            try
            {
                return Ok(await service.GetAllGreenitors());
            }
            catch (Exception ex)
            {
                return StatusCode(400, new ResponseMessage { Message = "Something went wrong" });
            }
        }

        /// <summary>
        /// Updates a user’s profile.
        /// </summary>
        /// <param name="username">The username of the user to update.</param>
        /// <param name="userDTO">The updated user data.</param>
        /// <returns>A response message indicating success or failure.</returns>
        [HttpPut("update/{username}")]
        public async Task<ActionResult<ResponseMessage>> UpdateUserProfile(string username, [FromForm] UpdateUserDTO userDTO)
        {
            try
            {
              
                var response = await service.UpdateUserProfile(username, userDTO);
                return Ok(response);
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
    }
}