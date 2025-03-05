using ForumAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ForumAPI.DTOs
{
    public class CreateEventDTO
    {
        public string Description { get; set; }


        public string Location { get; set; }


        public DateTime Date { get; set; }


        public string Status { get; set; }

    }
}
