using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMP.Domain.Entities.TemplateEntities
{
    public class TemplateSlide
    {
        public int Id { get; set; }
        public int SlideNumber { get; set; }
        public required string BackgroundType { get; set; }
        public required byte[] BackgroundValue { get; set; }
        public byte[] LogoValue { get; set; } = [];

        //navigation prop
        public List<TemplateSlideElement> Elements { get; set; } = [];
        public int TemplateId { get; set; }
        public Template Template { get; set; } = null!;
    }
}
