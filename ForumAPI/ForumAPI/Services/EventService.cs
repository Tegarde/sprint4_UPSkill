using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.Enums;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumAPI.Services
{
    /// <summary>
    /// Service responsible for managing events in the forum.
    /// </summary>
    public class EventService : EventDAO
    {
        private readonly DataContext context;
        private readonly GreenitorDAO greenitorClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="greenitorClient">DAO for user interactions.</param>
        public EventService(DataContext context, GreenitorDAO greenitorClient)
        {
            this.context = context;
            this.greenitorClient = greenitorClient;
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="ev">The event object to be created.</param>
        /// <returns>The created event.</returns>
        /// <exception cref="ArgumentException">Thrown if the event date is invalid.</exception>
        public async Task<Event> CreateEventAsync(Event ev)
        {
            if (ev.Date > DateTime.Now)
            {
                context.Events.Add(ev);
                await context.SaveChangesAsync();
                return ev;
            }
            else
            {
                throw new ArgumentException("Failed to schedule event, please insert a valid date");
            }
        }

        /// <summary>
        /// Retrieves an event by its ID.
        /// </summary>
        /// <param name="id">The ID of the event.</param>
        /// <returns>The event with the specified ID.</returns>
        /// <exception cref="NotFoundException">Thrown if the event is not found.</exception>
        public Event GetEventById(int id)
        {
            var ev = context.Events.Include(e => e.Comments).ThenInclude(c => c.Replies).FirstOrDefault(e => e.Id == id);
            if (ev == null)
            {
                throw new NotFoundException("Event not found");
            }
            return ev;
        }

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        /// <returns>A list of all events.</returns>
        public List<Event> GetAllEvents()
        {
            return context.Events.ToList();
        }

        /// <summary>
        /// Retrieves events based on their status.
        /// </summary>
        /// <param name="status">The status of the events to retrieve.</param>
        /// <returns>A list of events with the specified status.</returns>
        /// <exception cref="ArgumentException">Thrown if the provided status is invalid.</exception>
        /// <exception cref="NotFoundException">Thrown if no events with the given status are found.</exception>
        public List<Event> GetEventsByStatus(string status)
        {
            if (!Enum.TryParse<EventStatus>(status, true, out EventStatus eventStatus))
            {
                throw new ArgumentException("Invalid event status.", nameof(status));
            }

            var events = context.Events.Where(e => e.Status == eventStatus).ToList();

            if (!events.Any())
            {
                throw new NotFoundException("Events not found");
            }

            return events;
        }

        /// <summary>
        /// Changes the status of an event.
        /// </summary>
        /// <param name="id">The ID of the event.</param>
        /// <param name="status">The new status for the event.</param>
        /// <returns>A success message.</returns>
        /// <exception cref="NotFoundException">Thrown if the event is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if the status is invalid or unchanged.</exception>
        public string ChangeEventStatus(int id, string status)
        {
            var ev = GetEventById(id);
            if (ev == null)
            {
                throw new NotFoundException("Couldn't find event");
            }
            if (!Enum.TryParse<EventStatus>(status, true, out EventStatus eventStatus))
            {
                throw new ArgumentException("Invalid event status.", nameof(status));
            }
            if (ev.Status == eventStatus)
            {
                throw new ArgumentException("Event is already in that status");
            }
            ev.Status = eventStatus;
            context.SaveChangesAsync();
            return "Event status updated successfully";
        }

        /// <summary>
        /// Retrieves the number of events attended by a specific user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The number of events attended by the user.</returns>
        public async Task<int> GetEventStatisticsByUsername(string username)
        {
            return await context.Attendances
                .Where(e => e.User == username)
                .CountAsync();
        }

        /// <summary>
        /// Registers a user as attending an event.
        /// </summary>
        /// <param name="attendance">The attendance details.</param>
        /// <returns>The created attendance record.</returns>
        /// <exception cref="NotFoundException">Thrown if the event is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if the user is already attending the event.</exception>
        public async Task<Attendance> CreateAttendance(Attendance attendance)
        {
            await greenitorClient.GetUserByUsername(attendance.User);

            var ev = await context.Events.FirstOrDefaultAsync(e => e.Id == attendance.EventId);
            if (ev == null)
            {
                throw new NotFoundException("Event not found");
            }

            if (context.Attendances.Any(a => a.EventId == ev.Id && a.User == attendance.User))
            {
                throw new ArgumentException("User is already attending this event");
            }

            attendance.Event = ev;

            await greenitorClient.IncrementUserInteractions(attendance.User);

            context.Attendances.Add(attendance);
            await context.SaveChangesAsync();
            return attendance;
        }

        /// <summary>
        /// Removes a user's attendance from an event.
        /// </summary>
        /// <param name="attendance">The attendance details.</param>
        /// <exception cref="NotFoundException">Thrown if the attendance record is not found.</exception>
        public async Task UnattendEvent(Attendance attendance)
        {
            await greenitorClient.GetUserByUsername(attendance.User);

            var attendanceToDelete = await context.Attendances
                .FirstOrDefaultAsync(a => a.EventId == attendance.EventId && a.User == attendance.User);

            if (attendanceToDelete == null)
            {
                throw new NotFoundException("Attendance not found");
            }

            await greenitorClient.DecrementUserInteractions(attendance.User);

            context.Attendances.Remove(attendanceToDelete);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds an image URL to an event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="url">The URL of the image.</param>
        /// <exception cref="NotFoundException">Thrown if the event is not found.</exception>
        public async Task AddImage(int eventId, string url)
        {
            var ev = await context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (ev == null)
            {
                throw new NotFoundException("Event not found");
            }
            ev.Image = url;
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Checks if a user is attending a specific event.
        /// </summary>
        /// <param name="eventId">The ID of the event.</param>
        /// <param name="username">The username of the user.</param>
        /// <returns>True if the user is attending, otherwise false.</returns>
        /// <exception cref="NotFoundException">Thrown if the event is not found.</exception>
        public async Task<bool> isAttending(int eventId, string username)
        {
            var ev = await context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (ev == null)
            {
                throw new NotFoundException("Event not found");
            }
            return context.Attendances.Any(a => a.EventId == ev.Id && a.User == username);
        }
    }
}