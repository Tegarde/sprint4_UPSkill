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
        
        public Task<Event> CreateEventAsync(Event ev);

        public List<Event> GetAllEvents();

        public Event GetEventById(int id);

        public List<Event> GetEventsByStatus(string Status);

        public string ChangeEventStatus(int id, string status);

        public Task<int> GetEventStatisticsByUsername(string username);

        Task<Attendance> CreateAttendance(Attendance attendance);

        Task UnattendEvent(Attendance attendance);
    }
}
