using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoPost.Domain.Dtos
{
    public class PlatformConfig
    {
        public Platform Platform { get; set; } 
        public string? BackgroundImage { get; set; }
        public required Format Format { get; set; }
        public required List<string> Tones { get; set; }

        public required List<string> Guidelines { get; set; }
    }
}
