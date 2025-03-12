using ForumAPI.Models;

namespace ForumAPI.Interfaces
{
    /// <summary>
    /// Interface defining data access operations related to events.
    /// </summary>
    public interface EventDAO
    {
        /// <summary>
        /// Creates a new event in the system.
        /// </summary>
        /// <param name="ev">The event object containing the details of the new event.</param>
        /// <returns>A task representing the asynchronous operation, with the created <see cref="Event"/> as the result.</returns>
        public Task<Event> CreateEventAsync(Event ev);

        /// <summary>
        /// Retrieves a list of all events.
        /// </summary>
        /// <returns>A list of <see cref="Event"/> objects representing all events.</returns>
        public List<Event> GetAllEvents();

        /// <summary>
        /// Retrieves an event by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the event to retrieve.</param>
        /// <returns>The <see cref="Event"/> object corresponding to the provided event ID.</returns>
        public Event GetEventById(int id);

        /// <summary>
        /// Retrieves a list of events filtered by their status (e.g., upcoming, ongoing, completed).
        /// </summary>
        /// <param name="Status">The status of the events to retrieve (e.g., "upcoming", "completed").</param>
        /// <returns>A list of <see cref="Event"/> objects matching the given status.</returns>
        public List<Event> GetEventsByStatus(string Status);

        /// <summary>
        /// Changes the status of an event.
        /// </summary>
        /// <param name="id">The ID of the event whose status is being changed.</param>
        /// <param name="status">The new status to assign to the event (e.g., "completed", "ongoing").</param>
        /// <returns>A string message indicating the result of the status change operation.</returns>
        public string ChangeEventStatus(int id, string status);

        /// <summary>
        /// Retrieves statistics for a specific user's attendance at events.
        /// </summary>
        /// <param name="username">The username of the user for whom to fetch event statistics.</param>
        /// <returns>A task representing the asynchronous operation, with an integer value representing the statistics (e.g., number of attended events).</returns>
        public Task<int> GetEventStatisticsByUsername(string username);

        /// <summary>
        /// Adds an attendance record for a user at a specific event.
        /// </summary>
        /// <param name="attendance">The attendance object containing the event ID and user details.</param>
        /// <returns>A task representing the asynchronous operation, with the created <see cref="Attendance"/> as the result.</returns>
        Task<Attendance> CreateAttendance(Attendance attendance);

        /// <summary>
        /// Removes an attendance record, indicating that a user is no longer attending a specific event.
        /// </summary>
        /// <param name="attendance">The attendance object identifying the event and user to remove the attendance for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UnattendEvent(Attendance attendance);

        /// <summary>
        /// Adds an image to an event by specifying its ID and image URL.
        /// </summary>
        /// <param name="eventId">The ID of the event to associate the image with.</param>
        /// <param name="url">The URL of the image to add to the event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddImage(int eventId, string url);

        /// <summary>
        /// Checks if a user is attending a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event to check attendance for.</param>
        /// <param name="username">The username of the user whose attendance is being checked.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean result indicating whether the user is attending.</returns>
        Task<bool> isAttending(int eventId, string username);
    }
}