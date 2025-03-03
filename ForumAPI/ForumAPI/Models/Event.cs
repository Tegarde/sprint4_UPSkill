using ForumAPI.Enums;
using System.ComponentModel.DataAnnotations;
namespace ForumAPI.Models
{
    /// <summary>
    /// Represents an event in the forum.
    /// </summary>
    public class Event
    {   
        /// <summary>
        /// The unique identifier for the event.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The description of the event.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// The location of the event.
        /// </summary>
        [Required]
        public string Location { get; set; }

        /// <summary>
        /// The date of the event.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// The status of the event.
        /// </summary>
        [Required]
        public EventStatus Status { get; set; }

        /// <summary>
        /// The attendance of the event.
        /// </summary>
        public ICollection<string> Attendance { get; set; }

    }
}
