using ForumAPI.DTOs.EventDTOs;
using ForumAPI.Enums;
using ForumAPI.Models;

namespace ForumAPI.Mapper
{
    /// <summary>
    /// A helper class to map between various DTOs and the Event model.
    /// </summary>
    public class EventMapper
    {
        /// <summary>
        /// Converts an Event model to an EventDTO.
        /// </summary>
        /// <param name="ev">The Event model to be mapped.</param>
        /// <returns>An EventDTO with data from the provided Event model.</returns>
        public static EventDTO ToDTO(Event ev)
        {
            return new EventDTO
            {
                Id = ev.Id,
                Description = ev.Description,
                Location = ev.Location,
                Date = ev.Date,
                Image = ev.Image,
                Status = ev.Status
            };
        }

        /// <summary>
        /// Converts an Event model to an EventWithCommentsDTO, which includes the event's comments.
        /// </summary>
        /// <param name="ev">The Event model to be mapped.</param>
        /// <returns>An EventWithCommentsDTO with data from the provided Event model, including comments.</returns>
        public static EventWithCommentsDTO toEventWithCommentsDTO(Event ev)
        {
            return new EventWithCommentsDTO
            {
                Id = ev.Id,
                Description = ev.Description,
                Location = ev.Location,
                Date = ev.Date,
                Status = ev.Status,
                Image = ev.Image,
                Comments = ev.Comments
                    .Select(CommentMapper.ToCommentFromEventDTO)
                    .ToList()
            };
        }

        /// <summary>
        /// Converts a CreateEventDTO to an Event entity.
        /// </summary>
        /// <param name="evDTO">The CreateEventDTO to be mapped.</param>
        /// <returns>An Event entity with data from the provided CreateEventDTO.</returns>
        public static Event ToEntity(CreateEventDTO evDTO)
        {
            return new Event
            {
                Description = evDTO.Description,
                Location = evDTO.Location,
                Date = evDTO.Date,
                Status = EventStatus.OPEN
            };
        }

        /// <summary>
        /// Converts an EventAttendanceDTO to an Attendance entity.
        /// </summary>
        /// <param name="evDTO">The EventAttendanceDTO to be mapped.</param>
        /// <returns>An Attendance entity with data from the provided EventAttendanceDTO.</returns>
        public static Attendance ToEntity(EventAttendanceDTO evDTO)
        {
            return new Attendance
            {
                EventId = evDTO.EventId,
                User = evDTO.Username
            };
        }
    }
}