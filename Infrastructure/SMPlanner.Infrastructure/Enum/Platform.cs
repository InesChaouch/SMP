using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SMPlanner.Infrastructure.Enum;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Platform
{
    [Description("LinkedIn")]
    LinkedIn = 0,

    [Description("Facebook")]
    Facebook = 1,

    [Description("Instagram")]
    Instagram = 2,
}
