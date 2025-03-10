using ForumAPI.DTOs.EventDTOs;
using ForumAPI.DTOs.PostDTOs;
using ForumAPI.Enums;
using ForumAPI.Models;
using Microsoft.Extensions.Hosting;

namespace ForumAPI.Mapper
{
    public class EventMapper
    {
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
