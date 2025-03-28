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
        public required TemplateSlideElement Subtitle { get; set; }
        public required TemplateSlideElement Title { get; set; }
        public required List<TemplateSlideElement> Content { get; set; }
        public required string BackgroundType { get; set; }
        public byte[] BackgroundImage { get; set; } = [];
        public byte[] LogoValue { get; set; } = [];
    }
}
