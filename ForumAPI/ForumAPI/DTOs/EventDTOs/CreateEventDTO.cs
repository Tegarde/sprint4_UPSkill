namespace ForumAPI.DTOs.EventDTOs
{
    public class CreateEventDTO
    {
        public string Description { get; set; }


        public string Location { get; set; }


        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
