using ForumAPI.CustomExceptions;
using ForumAPI.DTOs;
using ForumAPI.Interfaces;
using ForumAPI.Mapper;
using ForumAPI.Models;
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

        [HttpGet]
        public ActionResult GetAllEvents(){
            try{
                var events = service.GetAllEvents();
                return Ok(events);
            }
            catch (Exception ex){
                return BadRequest(new ResponseMessage { Message = "An error occurred while getting the events" });
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetEventById(int id){
            try
            {
                var ev = service.GetEventById(id);
                return Ok(ev);
            }
            catch (NotFoundException ex){
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex){
                return BadRequest(new ResponseMessage { Message = "An error occurred while getting the event" });
            }
        }

        [HttpGet("status/{status}")]
        public ActionResult GetEventsByStatus( [FromRoute] string status){
            try{
                var events = service.GetEventsByStatus(status);
                return Ok(events);
            }catch (NotFoundException ex){
                return NotFound(new ResponseMessage { Message = ex.Message });
            }catch (ArgumentException ex){
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }catch (Exception ex){
                return BadRequest(new ResponseMessage { Message = "An error occurred while getting the events" });
            }
        }

        [HttpPut("status/{id}")]
        public ActionResult ChangeEventStatus([FromRoute] int id, [FromBody] string status){
            try{
                var message = service.ChangeEventStatus(id, status);
                return Ok(new ResponseMessage { Message = message });
            }catch(NotFoundException ex){
                return NotFound(new ResponseMessage { Message = ex.Message });
            }catch(ArgumentException ex){
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }catch(Exception ex){
                return BadRequest(new ResponseMessage { Message = "An error occurred while changing the event status" });
            }
        }
        
    }
}
