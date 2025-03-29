using System.Text;
using Newtonsoft.Json;
using SMPlanner.Infrastructure.Entities;

namespace SMPlanner.Infrastructure.Service.HttpRequest;
public class HttpRequestService
{
    private readonly HttpClient _client;

    public HttpRequestService(HttpClient client)
    {
        _client = client;
    }
    public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, string baseUrl, TRequest data)
    {
        var result = new ApiResponse<TResponse>();

        try
        {
            _client.BaseAddress = new Uri(baseUrl);

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (responseContent != "" || responseContent != null)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse<TResponse>>(responseContent);

                    if (apiResponse != null)
                    {
                        result.Succeeded = true;
                        result.Result = apiResponse.Result;
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Errors.Add("Response does not contain valid data.");
                    }
                }
            }
            else
            {
                result.Succeeded = false;
                result.Errors.Add($"StatusCode: {response.StatusCode}");
                result.Errors.Add(responseContent);
            }
        }
        catch (Exception ex)
        {
            result.Succeeded = false;
            result.Errors.Add(ex.Message);
        }

        return result;
    }
}
