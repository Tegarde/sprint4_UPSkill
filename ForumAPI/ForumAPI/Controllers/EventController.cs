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
    public class EventController : ControllerBase
    {
        private readonly EventDAO service;

        public EventController(EventDAO service)
        {
            this.service = service;
        }

        [HttpPost]
        public ActionResult CreateEvent([FromBody] CreateEventDTO eventDTO)
        {
            try
            {
                Event ev = service.CreateEvent(EventMapper.toEntity(eventDTO));
                return CreatedAtAction(nameof(CreateEvent), new ResponseMessage { Message = "Event created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "An error occurred while creating the event" });
            }
        }
    }
}
