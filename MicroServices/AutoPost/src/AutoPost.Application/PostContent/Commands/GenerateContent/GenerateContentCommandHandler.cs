using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SMP.Application.PostContent.Commands.GenerateContent;
using SMP.Domain.Enums;
using System.Text.RegularExpressions;

namespace SMP.Application.PostContent.Commands.GenerateContentCommandHandler
{
    public class GenerateContentCommandHandler : IRequestHandler<GenerateContentCommand, string>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly string _geminiToken;

        public GenerateContentCommandHandler(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _geminiToken = _config.GetValue<string>("Tokens:GeminiToken")!;
        }
        private const string GeminiUrlApi = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-001:generateContent";


        public async Task<string> Handle(GenerateContentCommand request, CancellationToken cancellationToken)
        {
            var generatedText = "";
            var sb = new StringBuilder();
            const int maxRetries = 5;

            if (request.Format == Format.Text)
            {
                sb.AppendLine($@"Generate an engaging {request.Platform} post in {request.Format} format for a {request.Category}. The topic is {request.Topic}.");
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
                sb.AppendLine($@"Generate a short and impactful paragraph (minimum 10 words and maximum 20 words) that can be embedded inside a {request.Platform} image post for {request.Category}.");
                sb.AppendLine($"The topic is: {request.Topic}.");
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
                sb.AppendLine($@"I want to create a {request.Platform} carousel post for {request.Category}.");
                sb.AppendLine($"The topic is: {request.Topic}.");
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

            var prompt = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new
                    {
                        text = sb.ToString()
                    }
                }
            }
        }
            };

            var json = JsonSerializer.Serialize(prompt);

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    using var apiRequest = new HttpRequestMessage(HttpMethod.Post, $"{GeminiUrlApi}?key={_geminiToken}")
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };

                    var response = await _httpClient.SendAsync(apiRequest, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        throw new ApplicationException($"Gemini API call failed: {response.StatusCode} - {error}");
                    }

                    var result = await response.Content.ReadAsStringAsync();

                    using var document = JsonDocument.Parse(result);
                    var root = document.RootElement;

                    generatedText = root
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    break; 
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("503") || ex.Message.Contains("overloaded"))
                    {
                        int delay = (int)Math.Pow(2, attempt);
                        Console.WriteLine($"Tentative {attempt + 1} échouée. Nouvelle tentative dans {delay} secondes...");
                        await Task.Delay(delay * 1000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

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
    
