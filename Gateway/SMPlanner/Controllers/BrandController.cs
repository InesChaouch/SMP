using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMPlanner.Infrastructure.Constantes;
using SMPlanner.Infrastructure.Entite;

namespace SMPlanner.Controllers;
[ApiController]
[Route("api/v1/[controller]")]

public class BrandController : ControllerBase
{
    private readonly ILogger<BrandController> _logger;
    private readonly HttpClient _client;

    public BrandController(ILogger<BrandController> logger, HttpClient client)
    {
        _logger = logger;
        _client = client;
    }

    //[HttpPost(Name = "CreateBrand")]
    //public async Task<ApiResponse<BrandDto>> CreateBrand([FromBody] BrandDto brandDto)
    //{
    //    ApiResponse<BrandDto> result = new ApiResponse<BrandDto>();
    //    return result;
    //}
}
