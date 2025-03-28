using SMP.Application.Interfaces;
using SMP.Application.PostContent.Commands.GenerateContent;
using SMP.Domain.Dtos;
using SMP.Domain.Enums;
using System.Text.Json;
using System.Net.Http.Json;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;

namespace SMP.Application.Posts.Commands.AutomatePosts
{
    public class CreatePostLinkedCommand : IRequest<bool>
    {
        public required string Category { get; set; }
        public required string Topic { get; set; }
        public required PlatformConfig Config { get; set; } 
        }

   
}
