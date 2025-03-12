using ForumAPI.Enums;

namespace ForumAPI.DTOs.EventDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for an event.
    /// </summary>
    public class EventDTO
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
        /// Gets or sets the URL of the event's image (if available).
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the event will occur.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the status of the event (e.g., upcoming, ongoing, completed).
        /// </summary>
        public EventStatus Status { get; set; }
    }
}