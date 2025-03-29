using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using AutoPost.Application.PostContent.Commands.GenerateContent;

namespace AutoPost.Web.Endpoints;

public class Content : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(GenerateContent);
    }

    public async Task<Results<Ok<string>, BadRequest<string>>> GenerateContent(
        [FromBody] GenerateContentCommand dto,
        ISender sender)
    {
        try
        {
            var result = await sender.Send(dto);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest($"Failed to generate content: {ex.Message}");
        }
    }
}
