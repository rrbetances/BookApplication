using BookApplication.Models.Dtos;

namespace BookApplication.Services.Interfaces
{
    public interface IBookService
    {
        Task<ResponseDto> GetAllAsync();
        Task<ResponseDto?> GetByIdAsync(int id);
        Task<ResponseDto?> CreateAsync(BookDto book);
        Task<ResponseDto?> UpdateAsync(BookDto book);
        Task<ResponseDto?> DeleteAsync(int id);
    }
}
