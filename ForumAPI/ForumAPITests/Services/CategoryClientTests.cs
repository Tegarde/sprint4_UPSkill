using ForumAPI;
using ForumAPI.DTOs;
using ForumAPI.DTOs.CategoryDTOs;
using ForumAPI.Services;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ForumAPITests.Services
{
    public class CategoryClientTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly CategoryClient _categoryClient;

        public CategoryClientTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _categoryClient = new CategoryClient(_httpClient);
        }

        [Fact]
        public async Task CreateCategory_Success()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Description = "Test Category" };
            var responseMessage = new ResponseMessage { Message = "Category created successfully" };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(responseMessage)
                });

            // Act
            var result = await _categoryClient.CreateCategory(categoryDTO);

            // Assert
            Assert.Equal(responseMessage.Message, result.Message);
        }

        /*
        [Fact]
        public async Task CreateCategory_Failure()
        {
            // Arrange
            var categoryDTO = new CategoryDTO { Description = "Test Category" };
            var responseMessage = new ResponseMessage { Message = "Category creation failed" };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = JsonContent.Create(responseMessage)
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ResponseStatusException>(() => _categoryClient.CreateCategory(categoryDTO));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal(responseMessage.Message, exception.Message);
        }
        */

        [Fact]
        public async Task DeleteCategory_Success()
        {
            // Arrange
            var description = "Test Category";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            await _categoryClient.DeleteCategory(description);

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri != null && req.RequestUri.ToString().Contains(description)),
                ItExpr.IsAny<CancellationToken>());
        }

        /*
        [Fact]
        public async Task DeleteCategory_Failure()
        {
            // Arrange
            var description = "Test Category";
            var responseMessage = new ResponseMessage { Message = "Category deletion failed" };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = JsonContent.Create(responseMessage)
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ResponseStatusException>(() => _categoryClient.DeleteCategory(description));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal(responseMessage.Message, exception.Message);
        }
        */

        [Fact]
        public async Task GetCategoryByDescription_Success()
        {
            // Arrange
            var description = "Test Category";
            var categoryDTO = new CategoryDTO { Description = description };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(categoryDTO)
                });

            // Act
            var result = await _categoryClient.GetCategoryByDescription(description);

            // Assert
            Assert.Equal(description, result.Description);
        }

        /*
        [Fact]
        public async Task GetCategoryByDescription_Failure()
        {
            // Arrange
            var description = "Test Category";
            var responseMessage = new ResponseMessage { Message = "Category not found" };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = JsonContent.Create(responseMessage)
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ResponseStatusException>(() => _categoryClient.GetCategoryByDescription(description));
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
            Assert.Equal(responseMessage.Message, exception.Message);
        }
        */

        [Fact]
        public async Task GetAllCategories_Success()
        {
            // Arrange
            var categories = new List<CategoryDTO>
            {
                new CategoryDTO { Description = "Category 1" },
                new CategoryDTO { Description = "Category 2" }
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(categories)
                });

            // Act
            var result = await _categoryClient.GetAllCategories();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Category 1", result[0].Description);
            Assert.Equal("Category 2", result[1].Description);
        }

        [Fact]
        public async Task GetAllCategories_NoContent()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            // Act
            var result = await _categoryClient.GetAllCategories();

            // Assert
            Assert.Empty(result);
        }

        /*
        [Fact]
        public async Task GetAllCategories_Failure()
        {
            // Arrange
            var responseMessage = new ResponseMessage { Message = "Error retrieving categories" };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = JsonContent.Create(responseMessage)
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ResponseStatusException>(() => _categoryClient.GetAllCategories());
            Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
            Assert.Equal(responseMessage.Message, exception.Message);
        }
        */
    }
}
