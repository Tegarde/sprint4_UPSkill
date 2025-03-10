using ForumAPI.CustomExceptions;
using ForumAPI.DTOs;
using ForumAPI.DTOs.EventDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Mapper;
using ForumAPI.Models;
using ForumAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventDAO service;
        private readonly FileUploadService fileUploadService;

        public EventController(EventDAO service, FileUploadService fileUploadService)
        {
            this.service = service;
            this.fileUploadService = fileUploadService;
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> CreateEvent(CreateEventDTO eventDTO)
        {

            try
            {   
                //CreateEventDTO eventDTO = new CreateEventDTO(description, location, date, image);
                Event ev = await service.CreateEventAsync(EventMapper.ToEntity(eventDTO));
                if (eventDTO.Image!= null) 
                { 
                    string url = await fileUploadService.UploadFileAsync(eventDTO.Image);
                    await service.AddImage(ev.Id, url);
                }
                
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
                var events = service.GetAllEvents().Select(e => EventMapper.ToDTO(e)).ToList();
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
                return Ok(EventMapper.toEventWithCommentsDTO(ev));
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
                return Ok(events.Select(e => EventMapper.ToDTO(e)).ToList());
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

        [HttpPost("attendance")]
        public async Task<ActionResult> AttendEvent([FromBody] EventAttendanceDTO attendEventDTO){
            try{
                var attendance = await service.CreateAttendance(EventMapper.ToEntity(attendEventDTO));
                return Ok(new ResponseMessage { Message = "Event attended successfully" });
            }
            catch(NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch(ResponseStatusException ex)
            {
                return StatusCode((int) ex.StatusCode, ex.ResponseMessage);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "An error occurred while attending the event" });
            }
        }

        [HttpDelete("attendance")]
        public async Task<ActionResult> CancelEventAttendance([FromBody] EventAttendanceDTO attendEventDTO){
            try{
                await service.UnattendEvent(EventMapper.ToEntity(attendEventDTO));
                return Ok(new ResponseMessage { Message = "Event attendance canceled successfully" });
            }catch(NotFoundException ex){
                return NotFound(new ResponseMessage { Message = ex.Message });
            }catch(ResponseStatusException ex){
                return StatusCode((int) ex.StatusCode, ex.ResponseMessage);
            }catch(Exception ex){
                return BadRequest(new ResponseMessage { Message = "An error occurred while canceling the event attendance" });
            }
        }
        
    }
}
