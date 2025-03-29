using Microsoft.AspNetCore.Mvc;
using SMPlanner.Infrastructure.Constants;
using SMPlanner.Infrastructure.Entities;
using SMPlanner.Infrastructure.Service.HttpRequest;

namespace SMPlanner.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly HttpClient _client;
    private readonly HttpRequestService _httpRequestService;

    public PostController(ILogger<PostController> logger, HttpClient client, HttpRequestService httpRequestService)
    {
        _logger = logger;
        _client = client;
        _httpRequestService = httpRequestService;
    }

    [HttpPost(Name = "CreatePost")]
    public async Task<ApiResponse<PostDto>> CreatePost([FromBody] PostDto postDto)
    {
       return await _httpRequestService.PostAsync<PostDto, PostDto>(ApiNamings.CreatePost, "https://localhost:5001", postDto);
    }
}
