using ForumAPI.DTOs.CommentDTOs;
using ForumAPI.Enums;

namespace ForumAPI.DTOs.EventDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for an event with associated comments.
    /// </summary>
    public class EventWithCommentsDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the event.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the location where the event will take place.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the event will occur.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the status of the event (e.g., planned, ongoing, completed).
        /// </summary>
        public EventStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the URL or file path of an image related to the event.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the list of comments associated with the event.
        /// Each comment is represented by the CommentFromEventDTO.
        /// </summary>
        public List<CommentFromEventDTO> Comments { get; set; }
    }
}