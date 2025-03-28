using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SMP.Application.Interfaces;

namespace SMP.Infrastructure.Apis.Gemini
{
    public class GeminiService(HttpClient httpClient, IConfiguration config) : IGeminiService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly List<string> _apiKeys = config.GetSection("Tokens:GeminiApiKeys").Get<List<string>>() ?? new List<string>();
        private int _currentKeyIndex = 0;
        private readonly int _maxRequestsPerWindow = 15;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);
        private readonly List<DateTime> _requestTimestamps = new();
        private int _requestCount = 0;

        private const string GeminiUrlApi =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro-001:generateContent";

        private string CurrentApiKey => _apiKeys[_currentKeyIndex];

        private void SwitchApiKey()
        {
            _currentKeyIndex = (_currentKeyIndex + 1) % _apiKeys.Count;
        }

        private async Task RespectRateLimitAsync()
        {
            var now = DateTime.UtcNow;
            _requestTimestamps.RemoveAll(t => now - t > _rateLimitWindow);

            if (_requestTimestamps.Count >= _maxRequestsPerWindow)
            {
                var sleepTime = _rateLimitWindow - (now - _requestTimestamps[0]);
                await Task.Delay(sleepTime);
            }

            _requestTimestamps.Add(DateTime.UtcNow);
        }

        public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken)
        {
            await RespectRateLimitAsync();

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            const int maxRetries = 5;

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    using var apiRequest = new HttpRequestMessage(HttpMethod.Post, $"{GeminiUrlApi}?key={CurrentApiKey}")
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
                    _requestCount++;

                    if (_requestCount % 1495 == 0)
                    {
                        SwitchApiKey();
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("503") || ex.Message.Contains("overloaded"))
                    {
                        int delay = (int)Math.Pow(2, attempt);
                        await Task.Delay(delay * 1000, cancellationToken);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            throw new Exception("Max retries exceeded.");
        }
    }
}
