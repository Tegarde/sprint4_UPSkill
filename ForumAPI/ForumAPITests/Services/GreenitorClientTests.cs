using ForumAPI.DTOs;
using ForumAPI.DTOs.GreenitorDTOs;
using ForumAPI.Services;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ForumAPI.Tests.Services
{
    public class GreenitorClientTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly GreenitorClient _greenitorClient;

        public GreenitorClientTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _greenitorClient = new GreenitorClient(httpClient);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnResponseMessage_WhenSuccessful()
        {
            // Arrange
            var registerUserDTO = new RegisterUserWithImageDTO { Username = "testuser", Email = "test@example.com", Password = "password" };
            var responseMessage = new ResponseMessage { Message = "User registered successfully" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(responseMessage))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _greenitorClient.RegisterUser(registerUserDTO);

            // Assert
            Assert.Equal(responseMessage.Message, result.Message);
        }

        [Fact]
        public async Task Login_ShouldReturnTokenDTO_WhenSuccessful()
        {
            // Arrange
            var loginDTO = new LoginDTO("test@example.com", "password");
            var tokenDTO = new TokenDTO { Username = "testuser", Role = "user" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(tokenDTO))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _greenitorClient.Login(loginDTO);

            // Assert
            Assert.Equal(tokenDTO.Username, result.Username);
            Assert.Equal(tokenDTO.Role, result.Role);
        }

        [Fact]
        public async Task GetUserByUsername_ShouldReturnGreenitorDTO_WhenSuccessful()
        {
            // Arrange
            var username = "testuser";
            var greenitorDTO = new GreenitorDTO(username, "test@example.com", 0);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(greenitorDTO))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _greenitorClient.GetUserByUsername(username);

            // Assert
            Assert.Equal(greenitorDTO.Username, result.Username);
            Assert.Equal(greenitorDTO.Email, result.Email);
        }

        [Fact]
        public async Task IncrementUserInteractions_ShouldNotThrowException_WhenSuccessful()
        {
            // Arrange
            var username = "testuser";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act & Assert
            await _greenitorClient.IncrementUserInteractions(username);
        }

        [Fact]
        public async Task DecrementUserInteractions_ShouldNotThrowException_WhenSuccessful()
        {
            // Arrange
            var username = "testuser";
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NoContent);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            // Act & Assert
            await _greenitorClient.DecrementUserInteractions(username);
        }
    }
}