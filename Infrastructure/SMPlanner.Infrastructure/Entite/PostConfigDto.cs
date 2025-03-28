namespace SMPlanner.Infrastructure.Entite;
public class PostConfigDto
{
    public required string Category { get; set; }
    public required string Topic { get; set; }
    public required List<PlatformConfig> PlatformConfigs { get; set; }
}
