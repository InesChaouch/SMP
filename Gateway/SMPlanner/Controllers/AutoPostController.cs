using System.ComponentModel;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMPlanner.Infrastructure.Constants;
using SMPlanner.Infrastructure.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SMPlanner.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class AutoPostController : ControllerBase
{

    private readonly ILogger<AutoPostController> _logger;
    private readonly HttpClient _client;

    public AutoPostController(ILogger<AutoPostController> logger, HttpClient client)
    {
        _logger = logger;
        _client = client;
    }

    [HttpPost(Name = "AutoPost")]
    public async Task<ApiResponse<PostConfigDto>> AutoPost([FromBody] PostConfigDto postDto)
    {
        ApiResponse<PostConfigDto> result = new ApiResponse<PostConfigDto>();
        try
        {
            _client.BaseAddress = new Uri("https://localhost:5001");
            var json = JsonConvert.SerializeObject(postDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(ApiNamings.AutoPost, content);
            // Read the response content as a string
            string responseContent = await response.Content.ReadAsStringAsync();
            // Check if the request was successful (status code in the 2xx range)
            if (response.IsSuccessStatusCode)
            {   
                if (responseContent != "" && responseContent != null)
                {
                    // Deserialize the entire response into an intermediate object
                    var responseObject = JsonConvert.DeserializeObject<ApiResponse<PostConfigDto>>(responseContent)?.Result;

                    if (responseObject != null)
                    {
                        result.Succeeded = true;
                        result.Result = responseObject;
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
                result.Errors.Add(response.StatusCode.ToString());
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
