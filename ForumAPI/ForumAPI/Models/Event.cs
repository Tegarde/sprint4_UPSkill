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
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// The location of the event.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Location { get; set; }

        public string? Image { get; set; }

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

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// The attendance of the event.
        /// </summary>
        public ICollection<Attendance> Attendance { get; set; } = new List<Attendance>();

    }
}
