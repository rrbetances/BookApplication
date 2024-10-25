using BookApplication.Models.Dtos;
using BookApplication.Services.Interfaces;
using BookApplication.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookApplication.Services
{
    public class BookService : IBookService
    {
        private readonly IBaseService baseService;

        public BookService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> CreateAsync(BookDto book)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.ApiType.POST,
                Url = Utility.BookAPIBase + $"/api/v1/books",
                Data = book
            })!;
        }

        public async Task<ResponseDto?> DeleteAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.ApiType.DELETE,
                Url = Utility.BookAPIBase + $"/api/v1/books/{id}"
            });
        }

        public async Task<ResponseDto?> GetAllAsync()
        {

            var result = await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.ApiType.GET,
                Url = Utility.BookAPIBase + $"/api/v1/books"
            })!;

            if (result.IsSuccess)
            {
                result.Result = JsonConvert.DeserializeObject<List<BookDto>>(result.Result.ToString());
            }

            return result;
        }

        public async Task<ResponseDto?> GetByIdAsync(int id)
        {
            var result = await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.ApiType.GET,
                Url = Utility.BookAPIBase + $"/api/v1/books/{id}"
            })!;

            if (result.Result != null && result.IsSuccess)
            {
                result.Result = JsonConvert.DeserializeObject<BookDto>(result.Result.ToString());
            }

            return result;
        }

        public async Task<ResponseDto?> UpdateAsync(BookDto book)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.ApiType.PUT,
                Url = Utility.BookAPIBase + $"/api/v1/books/{book.Id}",
                Data = book
            })!;
        }
    }
}
