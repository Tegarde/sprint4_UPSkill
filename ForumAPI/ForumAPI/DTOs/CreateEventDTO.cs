namespace ForumAPI.DTOs
{
    public class CreateEventDTO
    {
        public string Description { get; set; }


        public string Location { get; set; }


        public DateTime Date { get; set; } = DateTime.UtcNow;


        public string Status { get; set; }

    }
}
