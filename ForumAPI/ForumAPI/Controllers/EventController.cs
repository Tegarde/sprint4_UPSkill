using ForumAPI.CustomExceptions;
using ForumAPI.DTOs;
using ForumAPI.DTOs.EventDTOs;
using ForumAPI.Interfaces;
using ForumAPI.Mapper;
using ForumAPI.Models;
using ForumAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ForumAPI.Controllers
{
    /// <summary>
    /// Controller for managing events, including creating, retrieving, updating, and attending events.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Event Management")]
    public class EventController : ControllerBase
    {
        private readonly EventDAO service;
        private readonly FileUploadService fileUploadService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventController"/> class.
        /// </summary>
        /// <param name="service">The event service for event management operations.</param>
        /// <param name="fileUploadService">The file upload service for managing event images.</param>
        public EventController(EventDAO service, FileUploadService fileUploadService)
        {
            this.service = service;
            this.fileUploadService = fileUploadService;
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="eventDTO">The event data to create a new event.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        /// <response code="201">Returns the created event status message</response>
        /// <response code="400">If there was an error in the request</response>
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [SwaggerOperation(
            Summary = "Create a new event",
            Description = "This endpoint allows creating a new event with provided data."
        )]
        [SwaggerResponse(201, "Event created successfully.")]
        [SwaggerResponse(400, "Bad request.")]
        public async Task<ActionResult> CreateEvent(CreateEventDTO eventDTO)
        {
            try
            {
                Event ev = await service.CreateEventAsync(EventMapper.ToEntity(eventDTO));

                if (eventDTO.Image != null)
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

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        /// <returns>A list of all events.</returns>
        /// <response code="200">Returns the list of all events</response>
        /// <response code="400">If there was an error retrieving the events</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Retrieve all events",
            Description = "This endpoint returns a list of all events."
        )]
        [SwaggerResponse(200, "List of events retrieved successfully.")]
        [SwaggerResponse(400, "Bad request.")]
        public ActionResult GetAllEvents()
        {
            try
            {
                var events = service.GetAllEvents().Select(e => EventMapper.ToDTO(e)).ToList();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "An error occurred while getting the events" });
            }
        }

        /// <summary>
        /// Retrieves an event by its ID.
        /// </summary>
        /// <param name="id">The ID of the event to retrieve.</param>
        /// <returns>The event with the specified ID.</returns>
        /// <response code="200">Returns the event details</response>
        /// <response code="404">If the event with the specified ID is not found</response>
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Retrieve event by ID",
            Description = "This endpoint retrieves an event by its unique ID."
        )]
        [SwaggerResponse(200, "Event found successfully.")]
        [SwaggerResponse(404, "Event not found.")]
        public ActionResult GetEventById(int id)
        {
            try
            {
                var ev = service.GetEventById(id);
                return Ok(EventMapper.toEventWithCommentsDTO(ev));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "An error occurred while getting the event" });
            }
        }

        /// <summary>
        /// Retrieves events by their status.
        /// </summary>
        /// <param name="status">The status of the events to retrieve.</param>
        /// <returns>A list of events with the specified status.</returns>
        /// <response code="200">Returns the list of events with the specified status</response>
        /// <response code="404">If no events with the specified status are found</response>
        /// <response code="400">If there is a bad request (e.g. invalid status)</response>
        [HttpGet("status/{status}")]
        [SwaggerOperation(
            Summary = "Retrieve events by status",
            Description = "This endpoint retrieves events by their current status."
        )]
        [SwaggerResponse(200, "List of events with the specified status.")]
        [SwaggerResponse(404, "No events found with the specified status.")]
        [SwaggerResponse(400, "Invalid status.")]
        public ActionResult GetEventsByStatus([FromRoute] string status)
        {
            try
            {
                var events = service.GetEventsByStatus(status);
                return Ok(events.Select(e => EventMapper.ToDTO(e)).ToList());
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "An error occurred while getting the events" });
            }
        }

        /// <summary>
        /// Changes the status of an event.
        /// </summary>
        /// <param name="id">The ID of the event to update.</param>
        /// <param name="status">The new status of the event.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        /// <response code="200">Returns the status change message</response>
        /// <response code="404">If the event with the specified ID is not found</response>
        /// <response code="400">If there was a bad request</response>
        [HttpPut("status/{id}")]
        [SwaggerOperation(
            Summary = "Change event status",
            Description = "This endpoint allows changing the status of an event."
        )]
        [SwaggerResponse(200, "Event status updated successfully.")]
        [SwaggerResponse(404, "Event not found.")]
        [SwaggerResponse(400, "Bad request.")]
        public ActionResult ChangeEventStatus([FromRoute] int id, [FromBody] string status)
        {
            try
            {
                var message = service.ChangeEventStatus(id, status);
                return Ok(new ResponseMessage { Message = message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "An error occurred while changing the event status" });
            }
        }

        /// <summary>
        /// Marks a user as attending an event.
        /// </summary>
        /// <param name="attendEventDTO">The attendance information.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        /// <response code="200">Returns a success message for attending</response>
        /// <response code="404">If the event or user is not found</response>
        /// <response code="400">If there is an error with the request</response>
        [HttpPost("attend")]
        [SwaggerOperation(
            Summary = "Attend an event",
            Description = "This endpoint marks a user as attending an event."
        )]
        [SwaggerResponse(200, "Successfully marked as attending.")]
        [SwaggerResponse(404, "Event or user not found.")]
        [SwaggerResponse(400, "Bad request.")]
        public async Task<ActionResult> AttendEvent([FromBody] EventAttendanceDTO attendEventDTO)
        {
            try
            {
                var attendance = await service.CreateAttendance(EventMapper.ToEntity(attendEventDTO));
                return Ok(new ResponseMessage { Message = "Event attended successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "An error occurred while attending the event" });
            }
        }

        /// <summary>
        /// Cancels a user's attendance to an event.
        /// </summary>
        /// <param name="attendEventDTO">The attendance cancellation information.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        /// <response code="200">Returns a success message for canceling attendance</response>
        /// <response code="404">If the event or user is not found</response>
        /// <response code="400">If there is an error with the request</response>
        [HttpDelete("unattend")]
        [SwaggerOperation(
            Summary = "Cancel event attendance",
            Description = "This endpoint allows canceling a user's attendance to an event."
        )]
        [SwaggerResponse(200, "Event attendance canceled successfully.")]
        [SwaggerResponse(404, "Event or user not found.")]
        [SwaggerResponse(400, "Bad request.")]
        public async Task<ActionResult> UnattendEvent([FromBody] EventAttendanceDTO attendEventDTO)
        {
            try
            {
                await service.UnattendEvent(EventMapper.ToEntity(attendEventDTO));
                return Ok(new ResponseMessage { Message = "Event attendance canceled successfully" });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (ResponseStatusException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.ResponseMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "An error occurred while canceling the event attendance" });
            }
        }

        /// <summary>
        /// Checks if a user is attending a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="username">The username of the user.</param>
        /// <returns>A boolean indicating whether the user is attending the event.</returns>
        /// <response code="200">Returns the attendance status (true/false)</response>
        /// <response code="404">If the event or user is not found</response>
        /// <response code="400">If there is an error with the request</response>
        [HttpGet("isAttending/{eventId}/{username}")]
        [SwaggerOperation(
            Summary = "Check user attendance",
            Description = "This endpoint checks if a user is attending a specific event."
        )]
        [SwaggerResponse(200, "Returns attendance status.")]
        [SwaggerResponse(404, "Event or user not found.")]
        [SwaggerResponse(400, "Bad request.")]
        public async Task<ActionResult> IsAttending(int eventId, string username)
        {
            try
            {
                return Ok(await service.isAttending(eventId, username));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ResponseMessage { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseMessage { Message = "Something went wrong" });
            }
        }
    }
}
