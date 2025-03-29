using MediatR;
using AutoPost.Application.Common.Interfaces;
using AutoPost.Application.Interfaces;
using AutoPost.Application.PostContent.Commands.GenerateContent;
using AutoPost.Application.PostContent.Commands.CreatePost;
using AutoPost.Application.TodoItems.Commands.CreateTodoItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPost.Domain.Dtos;
using AutoPost.Domain.Enums;
using AutoPost.Application.Posts.Commands.AutomatePosts;

namespace AutoPost.Application.PostContent.Commands.AutomatePosts
{
    public record AutomatePostCommand : IRequest
    {
        public required string Category { get; set; }
        public required string Topic { get; set; }
        public required List<PlatformConfig> PlatformConfigs { get; set; }
    }

    public class AutomatePostsCommandHandler : IRequestHandler<AutomatePostCommand>
    {
        private readonly ISender _sender;
        public AutomatePostsCommandHandler(ISender sender)
        {
            _sender = sender;
        }

        public async Task Handle(AutomatePostCommand request, CancellationToken cancellationToken)
        {

            foreach (var platformConfig in request.PlatformConfigs)
            {
                if (platformConfig.Platform == Platform.LinkedIn)
                {
                    var createPostLinkedCommand = new CreatePostLinkedCommand
                    {
                        
                        Category = request.Category,
                        Topic = request.Topic,
                        Config = new PlatformConfig
                        {
                            Platform = platformConfig.Platform,
                            Format = platformConfig.Format,
                            Guidelines = platformConfig.Guidelines,
                            BackgroundImage = platformConfig.BackgroundImage,
                        }
                    };
                    var postId = await _sender.Send(createPostLinkedCommand, cancellationToken);
                }
            }

        }
        private async Task<bool> CreatePostLinkedIn(ISender sender, CreatePostLinkedCommand command)
        {
            var result = await sender.Send(command);

            return result;
        }
    }
}

   
