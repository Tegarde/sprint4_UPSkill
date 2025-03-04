using ForumAPI.DTOs;
using ForumAPI.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ForumAPI.Interfaces
{
    public interface EventDAO
    {
        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="ev">The data for the new event.</param>
        /// <returns>A response message indicating the success or failure of the operation.</returns>
        
        public Event CreateEvent(Event ev);
    }
}
