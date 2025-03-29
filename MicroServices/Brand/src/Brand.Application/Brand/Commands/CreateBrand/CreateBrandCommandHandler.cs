using Microsoft.Extensions.Configuration;
using AutoPost.Application.PostContent.Commands.GenerateContent;

namespace AutoPost.Application.PostContent.Commands.GenerateContentCommandHandler
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, string>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public CreateBrandCommandHandler(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        Task<string> IRequestHandler<CreateBrandCommand, string>.Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
    
