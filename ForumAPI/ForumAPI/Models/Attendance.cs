using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForumAPI.Models
{   
    /// <summary>
    /// Represents an attendance record for an event.
    /// </summary>
    public class Attendance
    {
        /// <summary>
        /// The event associated with the attendance record.
        /// </summary>
        [ForeignKey("Event")]
        public int EventId { get; set; }

        /// <summary>
        /// The event associated with the attendance record.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// The user associated with the attendance record.
        /// </summary>
        [MaxLength(100)]
        public string User { get; set; }
    }
}
