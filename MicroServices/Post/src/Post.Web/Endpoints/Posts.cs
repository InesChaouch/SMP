using Microsoft.AspNetCore.Http.HttpResults;
using AutoPost.Application.Posts.Commands.AutomatePosts;


public class Posts : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(CreatePost, "CreatePost");
    }

    public async Task<Results<Ok, BadRequest<string>>> CreatePost(
         CreatePostLinkedCommand dto,
        ISender sender)
    {
        try
        {
            await sender.Send(dto);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest($"Failed to create post: {ex.Message}");
        }
    }
}
