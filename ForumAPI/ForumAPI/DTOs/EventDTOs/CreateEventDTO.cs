namespace ForumAPI.DTOs.EventDTOs
{
    public class CreateEventDTO
    {
        public string Description { get; set; }


        public string Location { get; set; }

        private DateTime _date = DateTime.UtcNow;

        public DateTime Date
        {
            get => _date;
            set => _date = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        public IFormFile? Image { get; set; }

        public CreateEventDTO(string description, string location, DateTime date, IFormFile? image)
        {
            Description = description;
            Location = location;
            Date = date;
            Image = image;
        }
    }
}
