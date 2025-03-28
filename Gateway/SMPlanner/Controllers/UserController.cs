using Microsoft.AspNetCore.Mvc;

namespace SMPlanner.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly HttpClient _client;

    public UserController(ILogger<UserController> logger, HttpClient client)
    {
        _logger = logger;
        _client = client;
    }

    //[HttpPost(Name = "CreateUser")]
    //public async Task<ApiResponse<UserDto>> CreateBrand([FromBody] UserDto userDto)
    //{
    //    ApiResponse<UserDto> result = new ApiResponse<UserDto>();
    //    return result;
    //}
}
