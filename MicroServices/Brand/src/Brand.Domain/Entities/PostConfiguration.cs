using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP.Domain.Entities.Enums;

namespace SMP.Domain.Entities
{
    public class PostConfiguration
    {
        public int Id { get; set; }
        public required string Topic { get; set; }
        public required int PostTypeId { get; set; }
        public required int MediaId { get; set; }
        public required int BrandId { get; set; }
        public required int FrequencyId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required int CreatedBy { get; set; }
        public required int PostFormatId { get; set; }

        public required PostType PostType { get; set; }
        public required PublishFrequency Frequency { get; set; }
        
        //To be modified
        public string Brand { get; set; } = "ConsultimIt";
        public string Category { get; set; } = "IT";
    }
}
