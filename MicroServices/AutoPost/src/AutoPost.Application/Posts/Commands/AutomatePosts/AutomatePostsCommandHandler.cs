using SMP.Application.PostContent.Commands.AutomatePosts;
using SMP.Domain.Dtos;
using SMP.Domain.Enums;

namespace SMP.Application.Posts.Commands.AutomatePosts;

public class AutomatePostsCommandHandler(ISender sender) : IRequestHandler<AutomatePostsCommand>
{
    private readonly ISender _sender = sender;

    public async Task Handle(AutomatePostsCommand request, CancellationToken cancellationToken)
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
                        Tones = platformConfig.Tones,
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
