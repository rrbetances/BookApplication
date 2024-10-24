using BookApplication.Models.Dtos;

namespace BookApplication.Services.Interfaces
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto request);
    }
}
