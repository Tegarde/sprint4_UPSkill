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
    }
}
