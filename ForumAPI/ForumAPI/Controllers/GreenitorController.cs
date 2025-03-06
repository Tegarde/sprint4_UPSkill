﻿
using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.DTOs.PostDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreenitorController : ControllerBase
    {
        private readonly GreenitorDAO service;
        private readonly PostDAO postService;

        public GreenitorController(GreenitorDAO service, PostDAO postService)
        {
            this.service = service;
            this.postService = postService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage>> RegisterUser([FromBody] RegisterUserDTO greenitor)
        {
            try
            {
                ResponseMessage message = await service.RegisterUser(greenitor);
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

        [HttpPost("/login")]
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

        [HttpGet("/username/{username}")]
        public async Task<ActionResult<GreenitorWithoutRoleDTO>> GetUserByUsername(string username)
        {
            try
            {
                var greenitor = await service.GetUserByUsername(username);

                if (greenitor == null)
                {
                    return NotFound(new { Message = "User not found" });
                }

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
    }
}
