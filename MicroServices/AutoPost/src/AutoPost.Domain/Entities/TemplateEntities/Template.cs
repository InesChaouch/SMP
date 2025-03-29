using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP.Domain.Entities.Enums.PostFormat;
using SMP.Domain.Entities.PostEntities;

namespace SMP.Domain.Entities.TemplateEntities
{
    public class Template
    {
        public int Id { get; set; }
        public required string Format { get; set; }
        public required string BrandDomain { get; set; }
        public required List<TemplateSlide> TemplateSlides { get; set; }
        public List<Post> Posts { get; set; } = [];
    }
}
