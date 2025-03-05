﻿using ForumAPI.DTOs;
using ForumAPI.Enums;
using ForumAPI.Models;

namespace ForumAPI.Mapper
{
    public class EventMapper
    {
        public static CreateEventDTO toDTO(Event ev)
        {
            return new CreateEventDTO
            {
                Description = ev.Description,
                Location = ev.Location,
                Date = ev.Date,
                Status = ev.Status.ToString()
            };
        }

        public static Event toEntity(CreateEventDTO evDTO)
        {
            return new Event
            {
                Description = evDTO.Description,
                Location = evDTO.Location,
                Date = evDTO.Date,
                Status = Enum.Parse<EventStatus>(evDTO.Status)
            };
        }
        
        
    }
}
