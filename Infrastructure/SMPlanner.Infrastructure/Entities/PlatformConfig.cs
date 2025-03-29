using SMPlanner.Infrastructure.Enum;

namespace SMPlanner.Infrastructure.Entities;
public class PlatformConfig
{
    public Platform Platform { get; set; }
    public string? BackgroundImage { get; set; }
    public required List<string> Tones { get; set; }
    public required Format Format { get; set; }
    public required List<string> Guidelines { get; set; }
}
