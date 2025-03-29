using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP.Domain.Entities.Enums;
using SMP.Domain.Entities.Enums.PostFormat;

namespace SMP.Domain.Entities
{
    public class PostConfiguration
    {
        public int Id { get; set; }
        public required string Topic { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }

        public int BrandId { get; set; }
        //nav props
        public int PostFormatId { get; set; }
        public PostFormat PostFormat { get; set; } = null!;
        public int PostTypeId { get; set; }
        public PostType PostType { get; set; } = null!;
        public int FrequencyId { get; set; }
        public  PublishFrequency Frequency { get; set; } = null!;

        //To be modified
        public string Brand { get; set; } = "Consultim-It";
        public string Category { get; set; } = "IT";
    }
}
