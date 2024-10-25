using BookApplication.Models.Dtos;
using BookApplication.Services;
using BookApplication.Utilities;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace BookApplicationTest
{
    public class BaseServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly BaseService _baseService;

        public BaseServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpClientFactoryMock
                .Setup(x => x.CreateClient("BookApplication"))
                .Returns(_httpClient);

            _baseService = new BaseService(_httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task SendAsync_ReturnsSuccess_WhenApiResponseIsOk()
        {
            // Arrange
            var request = new RequestDto
            {
                Url = "http://example.com/api/resource",
                ApiType = Utility.ApiType.GET,
                Data = null // Use null for a GET request
            };

            var expectedResponse = new ResponseDto { IsSuccess = true, Result = new { Id = 1, Name = "Test" } };
            var jsonResponse = JsonConvert.SerializeObject(expectedResponse.Result);

            _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            })
            .Verifiable();

            // Act
            var result = await _baseService.SendAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task SendAsync_ReturnsNotFound_WhenApiResponseIsNotFound()
        {
            // Arrange
            var request = new RequestDto
            {
                Url = "http://example.com/api/resource",
                ApiType = Utility.ApiType.GET,
                Data = null
            };

            _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            })
            .Verifiable();

            // Act
            var result = await _baseService.SendAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Not Found", result.Message);
        }

        [Fact]
        public async Task SendAsync_ReturnsUnauthorized_WhenApiResponseIsNotFound()
        {
            // Arrange
            var request = new RequestDto
            {
                Url = "http://example.com/api/resource",
                ApiType = Utility.ApiType.GET,
                Data = null
            };

            _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            })
            .Verifiable();

            // Act
            var result = await _baseService.SendAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Unauthorized", result.Message);
        }
    }
}