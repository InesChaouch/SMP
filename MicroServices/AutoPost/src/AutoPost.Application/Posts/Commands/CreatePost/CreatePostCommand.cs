using SMPlanner.Infrastructure.Entities;

namespace AutoPost.Application.Posts.Commands.AutomatePosts
{
    public class CreatePostLinkedCommand : PostDto, IRequest<bool>
    {
    }
}
