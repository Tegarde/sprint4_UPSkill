using ForumAPI.DTOs.CommentDTOs;
using ForumAPI.Enums;

namespace ForumAPI.DTOs.EventDTOs
{
    public class EventWithCommentsDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        public EventStatus Status { get; set; }

        public List<CommentFromEventDTO> Comments { get; set; }
    }
}
