
using ForumAPI.DTOs;
using ForumAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            }catch(ResponseStatusException ex)
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
