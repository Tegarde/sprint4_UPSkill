using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.DTOs;
using ForumAPI.Enums;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumAPI.Services
{
   
    public class EventService : EventDAO
    {
        private readonly DataContext context;

        public EventService(DataContext context)
        {
            this.context = context;
        }
        public Event CreateEvent(Event ev)
        {
            if (ev.Date > DateTime.Now)
            {
                context.Events.Add(ev);
                context.SaveChangesAsync();
                return ev;
            }
            else
                {
                    throw new ArgumentException("Failed to schedule event, please insert valid date");
                }
        }

        public Event GetEventById(int id){
            var ev = context.Events.FirstOrDefault(e => e.Id == id);
            if(ev == null)
            {
                throw new NotFoundException("Event not found");
            }
            return ev;
        }

        public List<Event> GetAllEvents(){
            return context.Events.ToList();
        }

        public List<Event> GetEventsByStatus(string status)
        {
            // Try to parse the string to the EventStatus enum (ignoring case)
            //Enum.TryParse<EventStatus>(status, true, out EventStatus eventStatus):
            //This method attempts to convert the string status into an EventStatus enum value.
            //status: The string you want to convert (for example, "open" or "Closed").
            //true: This parameter tells the method to ignore case differences during conversion, so "open" and "OPEN" would both successfully convert to EventStatus.Open.
            //out EventStatus eventStatus: This is an output parameter that will hold the converted enum value if the parsing is successful.
            //If the parsing fails, the method will throw an ArgumentException. 
            if (!Enum.TryParse<EventStatus>(status, true, out EventStatus eventStatus))
            {
                throw new ArgumentException("Invalid event status.", nameof(status));
            }

            // Retrieve events that match the parsed enum value
            var events = context.Events.Where(e => e.Status == eventStatus).ToList();

            // Check if any events were found
            if (!events.Any())
            {
                throw new NotFoundException("Events not found");
            }

            return events;
        }

        public string ChangeEventStatus(int id, string status)
        {
            var ev = GetEventById(id);
            if(ev == null)
            {
                throw new NotFoundException("Couldn't find event");
            }
            if(!Enum.TryParse<EventStatus>(status, true, out EventStatus eventStatus))
            {
                throw new ArgumentException("Invalid event status.", nameof(status));
            }   
            ev.Status = eventStatus;
            context.SaveChangesAsync();
            return "Event status updated successfully";
        }

        public async Task<int> GetEventStatisticsByUsername(string username)
        {
            int attendanceCount = await context.Attendances
                .Where(e => e.User == username)
                .CountAsync();

            return attendanceCount;
        }


    }
}
