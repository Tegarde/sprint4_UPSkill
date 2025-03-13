using ForumAPI.CustomExceptions;
using ForumAPI.Data;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Enums;
using ForumAPI.Interfaces;
using ForumAPI.Models;
using ForumAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ForumAPITests.Utilities;

namespace ForumAPI.Tests.Services
{
    public class EventServiceTests
    {
        private readonly DataContext _context;
        private readonly Mock<GreenitorDAO> _mockGreenitorClient;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataContext(options);
            _mockGreenitorClient = new Mock<GreenitorDAO>();
            _eventService = new EventService(_context, _mockGreenitorClient.Object);
        }

        [Fact]
        public async Task CreateEventAsync_ValidEvent_ReturnsCreatedEvent()
        {
            // Arrange
            var newEvent = new Event
            {
                Date = DateTime.Now.AddDays(1),
                Description = "Test Event",
                Location = "Test Location"
            };

            // Act
            var result = await _eventService.CreateEventAsync(newEvent);

            // Assert
            Assert.Equal(newEvent, result);
            Assert.Contains(newEvent, _context.Events);
        }


        [Fact]
        public async Task CreateEventAsync_InvalidDate_ThrowsArgumentException()
        {
            // Arrange
            var newEvent = new Event { Date = DateTime.Now.AddDays(-1) };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _eventService.CreateEventAsync(newEvent));
        }

        [Fact]
        public void GetEventById_ExistingEvent_ReturnsEvent()
        {
            // Arrange
            var eventId = 1;
            var eventMock = new Event { Id = eventId };
            _context.Events.Add(eventMock);
            _context.SaveChanges();

            // Act
            var result = _eventService.GetEventById(eventId);

            // Assert
            Assert.Equal(eventId, result.Id);
        }

        [Fact]
        public void GetEventById_NonExistingEvent_ThrowsNotFoundException()
        {
            // Arrange
            var eventId = 1;

            // Act & Assert
            Assert.Throws<NotFoundException>(() => _eventService.GetEventById(eventId));
        }

        [Fact]
        public void ChangeEventStatus_ValidStatus_UpdatesStatus()
        {
            // Arrange
            var eventId = 1;
            var status = "CLOSED";
            var eventMock = new Event { Id = eventId, Status = EventStatus.OPEN };
            _context.Events.Add(eventMock);
            _context.SaveChanges();

            // Act
            var result = _eventService.ChangeEventStatus(eventId, status);

            // Assert
            Assert.Equal("Event status updated successfully", result);
            Assert.Equal(EventStatus.CLOSED, eventMock.Status);
        }

        [Fact]
        public void GetAllEvents_ReturnsListOfEvents()
        {
            // Arrange
            var events = new List<Event> { new Event(), new Event() };
            _context.Events.AddRange(events);
            _context.SaveChanges();

            // Act
            var result = _eventService.GetAllEvents();

            // Assert
            Assert.Equal(events.Count, result.Count);
        }

        [Fact]
        public void GetEventsByStatus_ValidStatus_ReturnsEvents()
        {
            // Arrange
            var status = "OPEN";
            var events = new List<Event> { new Event { Status = EventStatus.OPEN } };
            _context.Events.AddRange(events);
            _context.SaveChanges();

            // Act
            var result = _eventService.GetEventsByStatus(status);

            // Assert
            Assert.Equal(events.Count, result.Count);
        }

        [Fact]
        public void GetEventsByStatus_InvalidStatus_ThrowsArgumentException()
        {
            // Arrange
            var status = "INVALID";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _eventService.GetEventsByStatus(status));
        }

        [Fact]
        public void ChangeEventStatus_InvalidStatus_ThrowsArgumentException()
        {
            // Arrange
            var eventId = 1;
            var status = "INVALID";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _eventService.ChangeEventStatus(eventId, status));
        }
        
        [Fact]
        public async Task GetEventStatisticsByUsername_ReturnsCount()
        {
            // Arrange
            var username = "testuser";
            var count = 0;

            // Act
            var result = await _eventService.GetEventStatisticsByUsername(username);

            // Assert
            Assert.Equal(count, result);
        }

        [Fact]
            public async Task CreateAttendance_ValidAttendance_ReturnsAttendance()
            {
                // Arrange
                var attendance = new Attendance { EventId = 1, User = "testuser" };
                var eventMock = new Event { Id = 1 };
                _mockGreenitorClient.Setup(c => c.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

                // Act
                var result = await _eventService.CreateAttendance(attendance);

                // Assert
                Assert.Equal(attendance, result);
            }
        
        [Fact]
        public async Task CreateAttendance_EventNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var attendance = new Attendance { EventId = 1, User = "testuser" };
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _eventService.CreateAttendance(attendance));
        }
        
        [Fact]
        public async Task UnattendEvent_ValidAttendance_RemovesAttendance()
        {
            // Arrange
            var attendance = new Attendance { EventId = 1, User = "testuser" };
            var attendanceMock = new Attendance { EventId = 1, User = "testuser" };
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            // Act
            await _eventService.UnattendEvent(attendance);

        }

        [Fact]
        public async Task UnattendEvent_AttendanceNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var attendance = new Attendance { EventId = 1, User = "testuser" };
            _mockGreenitorClient.Setup(c => c.GetUserByUsername(It.IsAny<string>())).ReturnsAsync(new GreenitorDTO());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _eventService.UnattendEvent(attendance));
        }

        [Fact]
        public async Task AddImage_ValidEvent_AddsImage()
        {
            // Arrange
            var eventId = 1;
            var url = "http://example.com/image.jpg";
            var eventMock = new Event { Id = eventId };

            // Act
            await _eventService.AddImage(eventId, url);

            // Assert
            Assert.Equal(url, eventMock.Image);
        }

        [Fact]
        public async Task AddImage_EventNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var eventId = 1;
            var url = "http://example.com/image.jpg";


            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _eventService.AddImage(eventId, url));
        }

        [Fact]
        public async Task IsAttending_ValidEvent_ReturnsTrue()
        {
            // Arrange
            var eventId = 1;
            var username = "testuser";
            var eventMock = new Event { Id = eventId };

            // Act
            var result = await _eventService.isAttending(eventId, username);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsAttending_EventNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var eventId = 1;
            var username = "testuser";

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _eventService.isAttending(eventId, username));
        }
    }
}
