using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP.Domain.Entities.Enums;
using SMP.Domain.Entities.Enums.PostFormat;

namespace SMP.Domain.Entities.PostEntities
{
    public class Post
    {
        public int Id { get; set; }
        public required string Caption { get; set; }
        public required string Content { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } // add required later
        public required string ModifiesdBy { get; set; }
        public DateTime ModifiedOn { get; set; } // add required later
        public required string DeletedBy { get; set; }
        public DateTime DeletedOn { get; set; } // add required later

        //navigation props
        public int ChannelProfileId { get; set; }
        public ChannelProfile ChannelProfile { get; set; } = null!;
        public int PostFormatId { get; set; }
        public PostFormat PostFormat { get; set; } = null!;
        public int PostStatusId { get; set; }
        public PostStatus PostStatus { get; set; } = null!;
    }
}
