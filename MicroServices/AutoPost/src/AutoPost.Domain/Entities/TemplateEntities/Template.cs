using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP.Domain.Entities.Enums.PostFormat;

namespace SMP.Domain.Entities.TemplateEntities
{
    public class Template
    {
        public int Id { get; set; }
        public required PostFormat Format { get; set; }
        public required string BrandDomain { get; set; }

    }
}
