namespace SMPlanner.Infrastructure.Entite;
public class PostDto
{
    public required string Category { get; set; }
    public required string Topic { get; set; }
    public required PlatformConfig Config { get; set; }
}
