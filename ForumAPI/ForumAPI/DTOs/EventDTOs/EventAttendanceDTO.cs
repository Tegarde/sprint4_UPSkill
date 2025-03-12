namespace ForumAPI.DTOs.EventDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for attending an event.
    /// </summary>
    public class EventAttendanceDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user attending the event.
        /// </summary>
        public string Username { get; set; }
    }
}