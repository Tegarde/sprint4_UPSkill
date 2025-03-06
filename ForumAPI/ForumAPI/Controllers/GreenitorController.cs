
using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;
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

        public GreenitorController(GreenitorDAO service)
        {
            this.service = service;
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

        [HttpGet("{username}")]
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
    }
}
