using ForumAPI.Data;
using ForumAPI.DTOs;
using ForumAPI.Interfaces;
using ForumAPI.Models;

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
    }
}
