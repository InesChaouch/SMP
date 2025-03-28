namespace SMP.Domain.Entities.TemplateEntities;

public class TemplateSlideElement
{
    public int Id { get; set; }
    public required string TextFont { get; set; }
    public required int TextSize { get; set; }
    public required string TextWeight { get; set; }
    public required string TextColor { get; set; } 
    public required string HorizontalAlignment { get; set; } 
    public required string VerticalAlignment { get; set; } 
    public int XPadding { get; set; }
    public int YPadding { get; set; }
    public string Text { get; set; } = string.Empty;
}
