using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP.Domain.Entities.PostEntities;

namespace SMP.Domain.Entities.ChannelEntities;

public class Channel
{
    [Key]
    public required String Name { get; set; }
    public required string Icon { get; set; }
    public required bool IsConnected { get; set; }
    public required string RedirectUri { get; set; }

    public ICollection<ChannelProfile> ChannelProfiles { get; set; } = new HashSet<ChannelProfile>();

}
