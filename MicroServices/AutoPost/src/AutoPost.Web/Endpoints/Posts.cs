using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SMP.Application.PostContent.Commands.AutomatePosts;

public class Posts : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(AutomatePosts, "AutomatePosts");

    }

    public async Task<Results<Ok, BadRequest<string>>> AutomatePosts(
         AutomatePostsCommand dto,
        ISender sender)
    {
        try
        {
            await sender.Send(dto);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest($"Failed to automate posts: {ex.Message}");
        }
    }
}
