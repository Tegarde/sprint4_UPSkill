namespace ForumAPI.DTOs.EventDTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) used to create a new event.
    /// </summary>
    public class CreateEventDTO
    {
        /// <summary>
        /// Gets or sets the description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the location where the event will take place.
        /// </summary>
        public string Location { get; set; }

        private DateTime _date = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time of the event. 
        /// The date is stored in UTC format.
        /// </summary>
        public DateTime Date
        {
            get => _date;
            set => _date = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets or sets the image file associated with the event (optional).
        /// </summary>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateEventDTO"/> class.
        /// </summary>
        /// <param name="description">The description of the event.</param>
        /// <param name="location">The location of the event.</param>
        /// <param name="date">The date and time of the event.</param>
        /// <param name="image">The image for the event (optional).</param>
        public CreateEventDTO(string description, string location, DateTime date, IFormFile? image)
        {
            Description = description;
            Location = location;
            Date = date;
            Image = image;
        }

        /// <summary>
        /// Default constructor for the <see cref="CreateEventDTO"/> class.
        /// </summary>
        public CreateEventDTO() { }
    }
}
