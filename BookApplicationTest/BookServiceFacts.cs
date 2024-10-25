using BookApplication.Models.Dtos;
using BookApplication.Services;
using BookApplication.Services.Interfaces;
using Moq;
using Newtonsoft.Json;

namespace BookApplicationTest
{
    public class BookServiceFacts
    {
        private readonly Mock<IBaseService> _mockBaseService;
        private readonly BookService _bookService;

        public BookServiceFacts()
        {
            _mockBaseService = new Mock<IBaseService>();
            _bookService = new BookService(_mockBaseService.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidBook_ReturnsSuccessResponse()
        {
            var bookDto = new BookDto { Id = 1, Title = "Test Book" };
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = bookDto };

            _mockBaseService.Setup(x => x.SendAsync(It.IsAny<RequestDto>()))
                .ReturnsAsync(expectedResponse);

            var result = await _bookService.CreateAsync(bookDto);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(bookDto, result.Result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfBooks()
        {
            var books = new List<BookDto>
        {
            new BookDto { Id = 1, Title = "Test Book 1", PageCount = 100, Description = "Test", PublishDate = DateTime.Now, Excerpt = "AAA" },
            new BookDto { Id = 2, Title = "Test Book 2", PageCount = 100, Description = "Test", PublishDate = DateTime.Now, Excerpt = "AAA"  }
        };
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = JsonConvert.SerializeObject(books) };

            _mockBaseService.Setup(x => x.SendAsync(It.IsAny<RequestDto>()))
                .ReturnsAsync(expectedResponse);

            var result = await _bookService.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var returnedBooks = result.Result as List<BookDto>;
            Assert.Equal(2, returnedBooks.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsBook()
        {
            var bookDto = new BookDto { Id = 1, Title = "Test Book" };
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = JsonConvert.SerializeObject(bookDto) };

            _mockBaseService.Setup(x => x.SendAsync(It.IsAny<RequestDto>()))
                .ReturnsAsync(expectedResponse);

            var result = await _bookService.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var returnedBook = result.Result as BookDto;
            Assert.Equal(bookDto.Title, returnedBook.Title);
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            var expectedResponse = new ResponseDto { IsSuccess = true, Result = null };

            _mockBaseService.Setup(x => x.SendAsync(It.IsAny<RequestDto>()))
                .ReturnsAsync(expectedResponse);

            var result = await _bookService.DeleteAsync(1);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Create_ValidId_ReturnsOk()
        {
            var request = new BookDto { Id = 1, Title = "Test Book 1", PageCount = 100, Description = "Test", PublishDate = DateTime.Now, Excerpt = "AAA" };

            var expectedResponse = new ResponseDto { IsSuccess = true, Result = null};

            _mockBaseService.Setup(x => x.SendAsync(It.IsAny<RequestDto>()))
                .ReturnsAsync(expectedResponse);

            var result = await _bookService.CreateAsync(request);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
