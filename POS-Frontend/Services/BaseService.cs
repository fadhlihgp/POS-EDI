using System.Text;
using Newtonsoft.Json;
using POS_Frontend.Helpers;
using POS_Frontend.Models;
using POS_Frontend.Services.Interfaces;

namespace POS_Frontend.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenProvider _tokenProvider;
    public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
    }

    public async Task<ResponseVm?> SendAsync(RequestVm requestVm, bool withToken = true)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("PosApp");
            // Untuk request, set header, get data.
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
                    
            //Token (Passing token)
            if (withToken)
            {
                var token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }
            
            // Url
            message.RequestUri = new Uri(requestVm.Url, UriKind.Relative);
            if (requestVm.ApiType !=  null && !requestVm.IsFormMultipart)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestVm.Data), Encoding.UTF8,
                            "application/json");
            }
                    
            // Untuk menerima response
            HttpResponseMessage? apiResponse = null;
            
            // Cek apakah request berisi data untuk dikirim
            if (requestVm.Data != null && requestVm.IsFormMultipart)
            {
                var formContent = new MultipartFormDataContent();

                // Parsing data ke dalam bentuk form-data
                foreach (var item in (Dictionary<string, object>)requestVm.Data)
                {
                    if (item.Value is IFormFile file)
                    {
                        // Jika berupa file, tambahkan sebagai StreamContent
                        var streamContent = new StreamContent(file.OpenReadStream())
                        {
                            Headers = { ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType) }
                        };
                        formContent.Add(streamContent, item.Key, file.FileName);
                    }
                    else
                    {
                        // Tambahkan data string
                        formContent.Add(new StringContent(item.Value?.ToString() ?? string.Empty), item.Key);
                    }
                }
                message.Content = formContent;
            }
            
            switch (requestVm.ApiType)
            {
                case StaticData.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                case StaticData.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case StaticData.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }
            
            apiResponse = await client.SendAsync(message);
            // Meresponse http status code dari api
            
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var apiResponseDto = JsonConvert.DeserializeObject<ResponseApiVm>(apiContent);
            return new ResponseVm
            {
                Message = apiResponseDto.Message,
                IsSuccess = apiResponse.IsSuccessStatusCode,
                Data = apiResponseDto.Data
            };
        }
        catch (Exception e)
        {
            return new ResponseVm()
            {
                IsSuccess = false,
                Message = e.Message
            };
        }

    }
    
    public async Task<ResponseVm?> SendFormAsync(RequestVm requestVm, bool withToken = true)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient("PosApp");

            // Membuat request message
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");

            // Menambahkan token jika diperlukan
            if (withToken)
            {
                var token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }

            // URL API
            message.RequestUri = new Uri(requestVm.Url, UriKind.Relative);

            // Cek apakah request berisi data untuk dikirim
            if (requestVm.Data != null)
            {
                var formContent = new MultipartFormDataContent();

                // Parsing data ke dalam bentuk form-data
                foreach (var item in (Dictionary<string, object>)requestVm.Data)
                {
                    if (item.Value is IFormFile file)
                    {
                        // Jika berupa file, tambahkan sebagai StreamContent
                        var streamContent = new StreamContent(file.OpenReadStream())
                        {
                            Headers = { ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType) }
                        };
                        formContent.Add(streamContent, item.Key, file.FileName);
                    }
                    else
                    {
                        // Tambahkan data string
                        formContent.Add(new StringContent(item.Value?.ToString() ?? string.Empty), item.Key);
                    }
                }

                message.Content = formContent;
            }

            // Tentukan HTTP method berdasarkan ApiType
            message.Method = requestVm.ApiType switch
            {
                StaticData.ApiType.DELETE => HttpMethod.Delete,
                StaticData.ApiType.POST => HttpMethod.Post,
                StaticData.ApiType.PUT => HttpMethod.Put,
                _ => HttpMethod.Get
            };

            // Kirim request ke API
            HttpResponseMessage apiResponse = await client.SendAsync(message);

            // Membaca response
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var apiResponseDto = JsonConvert.DeserializeObject<ResponseApiVm>(apiContent);

            return new ResponseVm
            {
                Message = apiResponseDto?.Message,
                IsSuccess = apiResponse.IsSuccessStatusCode,
                Data = apiResponseDto?.Data
            };
        }
        catch (Exception e)
        {
            return new ResponseVm
            {
                IsSuccess = false,
                Message = e.Message
            };
        }
    }

}