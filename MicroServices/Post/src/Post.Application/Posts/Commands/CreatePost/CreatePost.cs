using AutoPost.Application.Interfaces;
using AutoPost.Application.PostContent.Commands.GenerateContent;
using AutoPost.Domain.Dtos;
using AutoPost.Domain.Enums;
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

namespace AutoPost.Application.Posts.Commands.AutomatePosts
{
    public record CreatePostLinkedCommand : IRequest<bool>
    {
        public required string Category { get; set; }
        public required string Topic { get; set; }
        public required PlatformConfig Config { get; set; } 
        }

    public class CreatePostLinkedCommandHandler(ILinkedInService linkedInService, HttpClient httpClient, ISender sender) : IRequestHandler<CreatePostLinkedCommand, bool>
    {
        private readonly ILinkedInService _linkedInService = linkedInService;
        private readonly HttpClient _httpClient = httpClient;
        private readonly ISender _sender = sender;

        public async Task<bool> Handle(CreatePostLinkedCommand request, CancellationToken cancellationToken)
        {
            var generateContentCommand = new GenerateContentCommand
            {
                Platform = request.Config.Platform,
                Format = request.Config.Format,
                Category = request.Category,
                Topic = request.Topic,
                Guidelines = request.Config.Guidelines,
            };

            var content = await GeneratePostContent(_sender, generateContentCommand);

            if (request.Config.Format == Format.Text)
            {
                _ = await _linkedInService.SharePostAsync(content);
            }
            else if (request.Config.Format == Format.Image)
            {
                var resultInitializeImage = await _linkedInService.InitializeImage();
                var resultToRead = await resultInitializeImage.Content.ReadFromJsonAsync<JsonElement>();

                var uploadUrl = resultToRead.GetProperty("value").GetProperty("uploadMechanism")
                    .GetProperty("com.linkedin.digitalmedia.uploading.MediaUploadHttpRequest")
                    .GetProperty("uploadUrl").GetString();

                var assetUrn = resultToRead.GetProperty("value").GetProperty("asset").GetString()?.Substring(25);

                byte[] backgroundBytes = !string.IsNullOrWhiteSpace(request.Config.BackgroundImage)
                    ? await _httpClient.GetByteArrayAsync(request.Config.BackgroundImage)
                    : await _httpClient.GetByteArrayAsync("https://thefusioneer.com/wp-content/uploads/2023/11/5-AI-Advancements-to-Expect-in-the-Next-10-Years-scaled.jpeg");
               
                using var image = Image.Load(backgroundBytes);

                var font = SystemFonts.CreateFont("Arial", 40);
                var textOptions = new RichTextOptions(font)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Origin = new PointF(image.Width / 2, image.Height / 2)
                };

                image.Mutate(ctx =>
                {
                    ctx.DrawText(textOptions, content, Color.White);
                });

                using var ms = new MemoryStream();
                await image.SaveAsJpegAsync(ms);
                var finalImageBytes = ms.ToArray();

                await _linkedInService.UploadImage(uploadUrl!, finalImageBytes);
                await _linkedInService.SharePostImageAsync(assetUrn!);
            }
            else if (request.Config.Format == Format.Document)
            {
                byte[] backgroundBytes = !string.IsNullOrWhiteSpace(request.Config.BackgroundImage)
                    ? await _httpClient.GetByteArrayAsync(request.Config.BackgroundImage)
                    : await _httpClient.GetByteArrayAsync("https://thefusioneer.com/wp-content/uploads/2023/11/5-AI-Advancements-to-Expect-in-the-Next-10-Years-scaled.jpeg");
           
                var document = new PdfDocument();
                
                var slides = JsonSerializer.Deserialize<List<Slide>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (slides == null || slides.Count == 0)
                    throw new InvalidOperationException("Generated content could not be parsed into valid slides.");

                var font = SystemFonts.CreateFont("Arial", 40);
                
                {
                    using var image = Image.Load<Rgba32>(backgroundBytes);
                    image.Mutate(x => x.Resize(1200, 627));
                    image.Mutate(ctx =>
                    {
                        var titleOptions = new RichTextOptions(font)
                        {
                            Origin = new PointF(image.Width / 2, image.Height / 2),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            WrappingLength = image.Width - 100
                        };
                        ctx.DrawText(titleOptions, generateContentCommand.Topic, Color.White);
                    });

                    using var ms = new MemoryStream();
                    await image.SaveAsPngAsync(ms, new PngEncoder { CompressionLevel = PngCompressionLevel.Level6 });
                    ms.Seek(0, SeekOrigin.Begin);

                    using var xImage = XImage.FromStream(ms);
                    var page = document.AddPage();
                    page.Width = XUnit.FromPoint(1200);
                    page.Height = XUnit.FromPoint(627);
                    using var gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawImage(xImage, 0, 0, page.Width.Point, page.Height.Point);
                }
               
                foreach (var slide in slides)
                {
                    using var image = Image.Load<Rgba32>(backgroundBytes);
                    image.Mutate(x => x.Resize(1200, 627));
                    image.Mutate(ctx =>
                    {
                        var titleOptions = new RichTextOptions(font)
                        {
                            Origin = new PointF(image.Width / 2, image.Height / 3),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            WrappingLength = image.Width - 100
                        };
                        ctx.DrawText(titleOptions, slide.Title, Color.White);

                        var contentOptions = new RichTextOptions(font)
                        {
                            Origin = new PointF(image.Width / 2, image.Height * 2 / 3),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            WrappingLength = image.Width - 100
                        };
                        ctx.DrawText(contentOptions, slide.Content, Color.LightGray);
                    });

                    using var ms = new MemoryStream();
                    await image.SaveAsPngAsync(ms, new PngEncoder { CompressionLevel = PngCompressionLevel.Level6 });
                    ms.Seek(0, SeekOrigin.Begin);

                    using var xImage = XImage.FromStream(ms);
                    var page = document.AddPage();
                    page.Width = XUnit.FromPoint(1200);
                    page.Height = XUnit.FromPoint(627);
                    using var gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawImage(xImage, 0, 0, page.Width.Point, page.Height.Point);
                }

                {
                    using var image = Image.Load<Rgba32>(backgroundBytes);
                    image.Mutate(x => x.Resize(1200, 627));
                    image.Mutate(ctx =>
                    {
                        var footerOptions = new RichTextOptions(font)
                        {
                            Origin = new PointF(image.Width / 2, image.Height / 2),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            WrappingLength = image.Width - 100
                        };
                        ctx.DrawText(footerOptions, "🚀 Thanks for reading!", Color.White);
                    });

                    using var ms = new MemoryStream();
                    await image.SaveAsPngAsync(ms, new PngEncoder { CompressionLevel = PngCompressionLevel.Level6 });
                    ms.Seek(0, SeekOrigin.Begin);

                    using var xImage = XImage.FromStream(ms);
                    var page = document.AddPage();
                    page.Width = XUnit.FromPoint(1200);
                    page.Height = XUnit.FromPoint(627);
                    using var gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawImage(xImage, 0, 0, page.Width.Point, page.Height.Point);
                }

                using var outputStream = new MemoryStream();
                document.Save(outputStream, false);
                var fileBytes = outputStream.ToArray();
                var resultInitializeDocument = await _linkedInService.InitializeDocument();
                var resultToRead = await resultInitializeDocument.Content.ReadFromJsonAsync<JsonElement>();

                var uploadUrl = resultToRead.GetProperty("value").GetProperty("uploadUrl").GetString();
                var assetUrn = resultToRead.GetProperty("value").GetProperty("document").GetString()?.Substring(16);
                await _linkedInService.UploadDocument(uploadUrl!, fileBytes);
                await _linkedInService.SharePostDocumentAsync(assetUrn!);
            }

            return true;
        }

        private static async Task<string> GeneratePostContent(ISender sender, GenerateContentCommand command)
        {
            var result = await sender.Send(command);
            return result;
        }
    }
}
