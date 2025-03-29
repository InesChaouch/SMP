using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using SMP.Application.Interfaces;
using SMPlanner.Infrastructure.Enum;

namespace SMP.Application.PostContent.Commands.GenerateContent
{
    public class GenerateContentCommandHandler(IGeminiService geminiService) : IRequestHandler<GenerateContentCommand, string>
    {
        private readonly IGeminiService _geminiService = geminiService;

        public async Task<string> Handle(GenerateContentCommand request, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            var tones ="";
            if(request.Tones.Count > 0) { 
            foreach (var tone in request.Tones)
            {
                sb.Append(tone).Append(",");
            }
            sb.Remove(sb.Length-1, 1);

            tones = sb.ToString();
            
            sb.Clear();
            }
            if (request.Format == Format.Text)
            {
                sb.AppendLine($"Generate an engaging {request.Platform} post in {request.Format} format for a {request.Category}. The topic is {request.Topic}.");
                if (tones !=null)
                {
                    sb.AppendLine($"Use this tone: {tones}");
                }
                sb.AppendLine("Rules:");
                sb.AppendLine("- Do NOT use bold text.");
                sb.AppendLine("- Use ALL CAPS for title and section headings.");
                sb.AppendLine("- Use line breaks between sections for readability.");
                sb.AppendLine("- Include emojis when needed.");
                sb.AppendLine("- End with a strong call-to-action (CTA) to encourage engagement.");
                sb.AppendLine("- Include a topic relevant hashtag section at the end of the post.");
                foreach (var guideline in request.Guidelines)
                {
                    sb.AppendLine($"- {guideline}");
                }
            }
            else if (request.Format == Format.Image)
            {
                sb.AppendLine($"Generate a short and impactful paragraph (minimum 10 words and maximum 20 words) that can be embedded inside a {request.Platform} image post for {request.Category}.");
                sb.AppendLine($"The topic is: {request.Topic}.");
                if (tones != null)
                {
                    sb.AppendLine($"Use this tone: {tones}");
                }
                sb.AppendLine("Rules:");
                sb.AppendLine("- No hashtags, no emojis, no links.");
                sb.AppendLine("- Do NOT use bold text.");
                sb.AppendLine("- It must be self-contained and clear even without caption.");
                sb.AppendLine("- Tone: Bold, suitable for a professional audience.");
                if (request.Guidelines != null && request.Guidelines.Any())
                {
                    foreach (var guideline in request.Guidelines)
                    {
                        sb.AppendLine($"- {guideline}");
                    }
                }
                sb.AppendLine("The goal is to catch attention visually when the post is viewed in-feed.");
            }
            else if (request.Format == Format.Document)
            {
                sb.AppendLine($"I want to create a {request.Platform} carousel post for {request.Category}.");
                sb.AppendLine($"The topic is: {request.Topic}.");
                if (tones != null)
                {
                    sb.AppendLine($"Use this tone: {tones}");
                }
                sb.AppendLine("Please generate a carousel with the following for each slide:");
                sb.AppendLine("- A title (short and engaging, max 8 words)");
                sb.AppendLine("- A brief content paragraph (2–4 sentences, concise, easy to read)");
                sb.AppendLine("Rules:");
                sb.AppendLine("- No hashtags, no emojis.");
                sb.AppendLine("- It must be self-contained and clear even without caption.");
                sb.AppendLine("- Tone: suitable for a professional audience.");
                if (request.Guidelines != null && request.Guidelines.Any())
                {
                    foreach (var guideline in request.Guidelines)
                    {
                        sb.AppendLine($"- {guideline}");
                    }
                }
                sb.AppendLine("The goal is to catch attention visually when the post is viewed in-feed.");
            }

            var result = await _geminiService.GenerateAsync(sb.ToString(), cancellationToken);

            using var document = JsonDocument.Parse(result);
            var root = document.RootElement;

            var textElement = root
    .GetProperty("candidates")[0]
    .GetProperty("content")
    .GetProperty("parts")[0]
    .GetProperty("text");

            string generatedText = textElement.GetString() ?? string.Empty;

            if (request.Format == Format.Document)
            {
                var slides = new List<object>();

                var regex = new Regex(@"\*\*Slide\s*(\d+)\*\*\s*\n\n\*\*Title:\*\*\s*(.+?)\n\n\*\*Content:\*\*\s*(.+?)(?=\n\n\*\*Slide|\n*$)", RegexOptions.Singleline);
                var matches = regex.Matches(generatedText ?? "");

                foreach (Match match in matches)
                {
                    slides.Add(new
                    {
                        SlideNumber = int.Parse(match.Groups[1].Value),
                        Title = match.Groups[2].Value.Trim(),
                        Content = match.Groups[3].Value.Trim()
                    });
                }

                return JsonSerializer.Serialize(slides, new JsonSerializerOptions
                {
                    WriteIndented = false
                });
            }
            else
            {
                return generatedText ?? "";
            }
        }
    }
}
