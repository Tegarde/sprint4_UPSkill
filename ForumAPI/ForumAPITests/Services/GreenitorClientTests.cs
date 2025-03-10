using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ForumAPI.DTOs;
using ForumAPI.DTOs.BadgeDTOs;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Services;
using Moq;
using Moq.Protected;
using Xunit;

namespace ForumAPI.Tests.Services
{
    public class GreenitorClientTests
    {
        private readonly GreenitorClient _greenitorClient;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public GreenitorClientTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost:8080/api/greenitor")
            };
            _greenitorClient = new GreenitorClient(httpClient);
        }

        [Fact]
        public async Task RegisterUser_ValidRequest_ReturnsResponseMessage()
        {
            // Arrange
            var registerUserDTO = new RegisterUserWithImageDTO { Username = "testuser", Email = "test@example.com", Password = "password" };
            var responseMessage = new ResponseMessage { Message = "User registered successfully" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseMessage))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _greenitorClient.RegisterUser(registerUserDTO);

            // Assert
            Assert.Equal(responseMessage.Message, result.Message);
        }

        [Fact]
        public async Task Login_ValidRequest_ReturnsTokenDTO()
        {
            // Arrange
            var loginDTO = new LoginDTO("test@example.com", "password");
            var tokenDTO = new TokenDTO { Username = "testuser", Role = "user" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(tokenDTO))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _greenitorClient.Login(loginDTO);

            // Assert
            Assert.Equal(tokenDTO.Username, result.Username);
            Assert.Equal(tokenDTO.Role, result.Role);
        }

        [Fact]
        public async Task GetUserByUsername_ValidRequest_ReturnsGreenitorDTO()
        {
            // Arrange
            var username = "testuser";
            var greenitorDTO = new GreenitorDTO(username, "test@example.com", 5)
            {
                Role = "user",
                Image = null,
                Badges = new List<BadgeDescriptionDTO>()
            };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(greenitorDTO))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _greenitorClient.GetUserByUsername(username);

            // Assert
            Assert.Equal(greenitorDTO.Username, result.Username);
            Assert.Equal(greenitorDTO.Email, result.Email);
            Assert.Equal(greenitorDTO.Interactions, result.Interactions);
        }

        [Fact]
        public async Task IncrementUserInteractions_ValidRequest_Success()
        {
            // Arrange
            var username = "testuser";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            await _greenitorClient.IncrementUserInteractions(username);

            // Assert
            _httpMessageHandlerMock.Protected()
                .Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DecrementUserInteractions_ValidRequest_Success()
        {
            // Arrange
            var username = "testuser";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            await _greenitorClient.DecrementUserInteractions(username);

            // Assert
            _httpMessageHandlerMock.Protected()
                .Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
        }
    }
}
