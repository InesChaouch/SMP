using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SMP.Application.Common.Interfaces;
using SMP.Application.Interfaces;
using SMP.Application.PostContent.Commands.GenerateContent;
using SMP.Application.Posts.Commands.AutomatePosts;
using SMP.Domain.Dtos;
using SMP.Domain.Enums;
using System.Net.Http.Json;
using System.Text.Json;

public class CreatePostLinkedCommandHandler(ILinkedInService linkedInService, HttpClient httpClient, ISender sender, IApplicationDbContext context) : IRequestHandler<CreatePostLinkedCommand, bool>
{
    private readonly ILinkedInService _linkedInService = linkedInService;
    private readonly HttpClient _httpClient = httpClient;
    private readonly ISender _sender = sender;
    private readonly IApplicationDbContext _context = context;


    public async Task<bool> Handle(CreatePostLinkedCommand request, CancellationToken cancellationToken)
    {
        var generateContentCommand = new GenerateContentCommand
        {
            Platform = request.Config.Platform,
            Tones = request.Config.Tones,
            Format = request.Config.Format,
            Category = request.Category,
            Topic = request.Topic,
            Guidelines = request.Config.Guidelines,
        };

        var content = await GeneratePostContent(_sender, generateContentCommand);

        if (request.Config.Format == Format.Document)
        {
            var slides = JsonSerializer.Deserialize<List<Slide>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (slides == null) throw new Exception("Slides content empty!");
            var template = await _context.Templates
                .Include(t => t.TemplateSlides)
                    .ThenInclude(s => s.Elements)
                .FirstOrDefaultAsync(t => t.Id == 18, cancellationToken);

            if (template == null)
                throw new Exception("Template not found");

            foreach (var slide in slides)
            {
                var templateSlide = template.TemplateSlides.FirstOrDefault(ts => ts.SlideNumber == slide.SlideNumber);
                if (templateSlide == null)
                    continue;

                var title = templateSlide.Elements.FirstOrDefault(e => e.ElementType == "Title");
                if (title != null) title.Text = "AI";

                var subtitle = templateSlide.Elements.FirstOrDefault(e => e.ElementType == "Subtitle");
                if (subtitle != null) subtitle.Text = slide.Title;

                var contentElement = templateSlide.Elements.FirstOrDefault(e => e.ElementType == "Content");
                if (contentElement != null) contentElement.Text = slide.Content;
            }

            await _context.SaveChangesAsync(cancellationToken);
            var document = new PdfDocument();

            foreach (var slide in template.TemplateSlides)
            {
                var background = slide.BackgroundValue?.Length > 0 ? slide.BackgroundValue :
                    await _httpClient.GetByteArrayAsync("https://thefusioneer.com/wp-content/uploads/2023/11/5-AI-Advancements-to-Expect-in-the-Next-10-Years-scaled.jpeg");

                using var image = Image.Load<Rgba32>(background);
                image.Mutate(x => x.Resize(1200, 627));

                foreach (var element in slide.Elements)
                {
                    var font = SystemFonts.CreateFont(element.TextFont, element.TextSize);
                    var options = new RichTextOptions(font)
                    {
                        Origin = new PointF(
                            element.HorizontalAlignment switch
                            {
                                "Left" => element.XPadding,
                                "Right" => image.Width - element.XPadding,
                                _ => image.Width / 2
                            },
                            element.VerticalAlignment switch
                            {
                                "Top" => element.YPadding,
                                "Bottom" => image.Height - element.YPadding,
                                _ => image.Height / 2
                            }),
                        HorizontalAlignment = element.HorizontalAlignment switch
                        {
                            "Left" => HorizontalAlignment.Left,
                            "Right" => HorizontalAlignment.Right,
                            _ => HorizontalAlignment.Center
                        },
                        VerticalAlignment = element.VerticalAlignment switch
                        {
                            "Top" => VerticalAlignment.Top,
                            "Bottom" => VerticalAlignment.Bottom,
                            _ => VerticalAlignment.Center
                        },
                        WrappingLength = image.Width - (element.XPadding * 2)
                    };

                    image.Mutate(ctx =>
                    {
                        ctx.DrawText(options, element.Text, Color.ParseHex(element.TextColor));
                    });
                }

                //if (slide.LogoValue?.Length > 0)
                //{
                //    using var logoImage = Image.Load<Rgba32>(slide.LogoValue);
                //    logoImage.Mutate(l => l.Resize(100, 100));
                //    image.Mutate(ctx => ctx.DrawImage(logoImage, new Point(image.Width - 110, 20), 1f));
                //}

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
