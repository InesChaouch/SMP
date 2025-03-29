using SMPlanner.Infrastructure.Entities;

namespace SMP.Application.Posts.Commands.AutomatePosts
{
    public class CreatePostLinkedCommand : PostDto, IRequest<bool>
    {
    }
}
