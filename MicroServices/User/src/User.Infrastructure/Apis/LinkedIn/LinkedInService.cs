using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using AutoPost.Infrastructure.Settings;
using AutoPost.Application.Interfaces;

namespace AutoPost.Infrastructure.Apis.LinkedIn
{
    public class LinkedInService : ILinkedInService
    {
        private readonly LinkedInSettings _linkedInSettings;
        private readonly HttpClient _httpClient;


        public LinkedInService(IOptions<LinkedInSettings> linkedInSettings, HttpClient httpClient)
        {
            _linkedInSettings = linkedInSettings.Value;
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> SharePostAsync(string text)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/v2/ugcPosts");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _linkedInSettings.AccessToken);
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");

            var payload = new
            {
                author = $"urn:li:person:{_linkedInSettings.UserUrn}",
                lifecycleState = "PUBLISHED",
                specificContent = new
                {
                    shareContent = new
                    {
                        shareCommentary = new { text },
                        shareMediaCategory = "NONE"
                    }
                },
                visibility = new
                {
                    memberVisibility = "CONNECTIONS"
                }
            };

            var json = JsonSerializer.Serialize(payload)
                                     .Replace("shareContent", "com.linkedin.ugc.ShareContent")
                                     .Replace("memberVisibility", "com.linkedin.ugc.MemberNetworkVisibility");

            request.Content = new StringContent(json);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> InitializeImage()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/v2/assets?action=registerUpload");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _linkedInSettings.AccessToken);
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");

            var payload = new
            {
                registerUploadRequest = new
                {
                    recipes = new[] { "urn:li:digitalmediaRecipe:feedshare-image" },
                    owner = $"urn:li:person:{_linkedInSettings.UserUrn}",
                    serviceRelationships = new[]
        {
            new
            {
                relationshipType = "OWNER",
                identifier = "urn:li:userGeneratedContent"
            }
        }
                }
            }; 

            request.Content = JsonContent.Create(payload);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> UploadImage(string uploadUrl, byte[] imageBytes)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uploadUrl);
            request.Content = new ByteArrayContent(imageBytes);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> SharePostImageAsync(string imageId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/v2/ugcPosts");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _linkedInSettings.AccessToken);
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");


            var payload = new
            {
                author = $"urn:li:person:{_linkedInSettings.UserUrn}",
                lifecycleState = "PUBLISHED",
                specificContent = new
                {
                    shareContent = new
                    {
                        shareCommentary = new { text = "" },
                        shareMediaCategory = "IMAGE",
                        media = new[] {
        new{
                status = "READY",
          media = $"urn:li:digitalmediaAsset:{imageId}"
        } 
                        }
                    }
                },
                visibility = new
                {
                    memberVisibility = "CONNECTIONS"
                }
            };

            var json = JsonSerializer.Serialize(payload)
                                     .Replace("shareContent", "com.linkedin.ugc.ShareContent")
                                     .Replace("memberVisibility", "com.linkedin.ugc.MemberNetworkVisibility");

            request.Content = new StringContent(json);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> InitializeDocument()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/rest/documents?action=initializeUpload");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _linkedInSettings.AccessToken);
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");
            request.Headers.Add("LinkedIn-Version", "202503");


            var payload = new
            { 
                initializeUploadRequest = new {
                owner = $"urn:li:person:{_linkedInSettings.UserUrn}",
                
            }
            };
            
            request.Content = JsonContent.Create(payload);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> UploadDocument(string uploadUrl, byte[] documentBytes)
        {

            var request = new HttpRequestMessage(HttpMethod.Put, uploadUrl);
            request.Headers.Add("LinkedIn-Version", "202503");

            request.Content = new ByteArrayContent(documentBytes);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

        public async Task<HttpResponseMessage> SharePostDocumentAsync(string documentId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/rest/posts");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _linkedInSettings.AccessToken);
            request.Headers.Add("X-Restli-Protocol-Version", "2.0.0");
            request.Headers.Add("LinkedIn-Version", "202503");

            var payload = new
            {
                author = $"urn:li:person:{_linkedInSettings.UserUrn}",
                commentary = "",
                visibility = "CONNECTIONS",
                distribution = new
                {
                    feedDistribution = "MAIN_FEED",
                    targetEntities = new Object[] {},
                    thirdPartyDistributionChannels = new Object[]{},
                },
                content = new
                {
                    media = new
                    {
                        title = "My PDF Document",
                        id = $"urn:li:document:{documentId}"
                    } },
                lifecycleState = "PUBLISHED",
                isReshareDisabledByAuthor = false,
            };

          
            request.Content = JsonContent.Create(payload);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response;
        }

    }
}
