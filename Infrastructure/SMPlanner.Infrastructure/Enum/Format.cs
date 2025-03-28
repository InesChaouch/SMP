using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SMPlanner.Infrastructure.Enum;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Format
{
    [Description("Text")]
    Text = 0,

    [Description("Image")]
    Image = 1,

    [Description("Document")]
    Document = 2
}
