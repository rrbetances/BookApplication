using BookApplication.Models.Dtos;
using BookApplication.Services.Interfaces;
using BookApplication.Utilities;
using Newtonsoft.Json;
using System.Text;

namespace BookApplication.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto> SendAsync(RequestDto request)
        {
            try
            {
                HttpClient client = httpClientFactory.CreateClient("BookApplication");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                message.RequestUri = new Uri(request.Url);

                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                switch (request.ApiType)
                {
                    case Utility.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Utility.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Utility.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        return new ResponseDto { IsSuccess = false, Message = "Not Found" };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new ResponseDto { IsSuccess = false, Message = "Access Denied" };
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new ResponseDto { IsSuccess = false, Message = "Unauthorized" };
                    case System.Net.HttpStatusCode.UnsupportedMediaType:
                        return new ResponseDto { IsSuccess = false, Message = "Unsoported media type" };
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new ResponseDto { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = new ResponseDto();

                        if (apiResponseDto.Result != null)
                            apiResponseDto.Result = JsonConvert.DeserializeObject<object>(apiContent);

                        return apiResponseDto;

                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto { IsSuccess = false, Message = ex.Message };

                return dto;
            };
        }
    }
}
