using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPost.Domain.Dtos
{
    public class Slide
    {
        public int SlideNumber { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public string BackgroundImageUrl { get; set; } = "";
    }
}
