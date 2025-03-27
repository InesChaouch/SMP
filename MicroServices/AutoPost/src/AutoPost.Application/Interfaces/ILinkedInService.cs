using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMP.Application.Interfaces
{
    public interface ILinkedInService
    {
        public Task<HttpResponseMessage> SharePostAsync(string message);
        public Task<HttpResponseMessage> InitializeImage();
        public Task<HttpResponseMessage> UploadImage(string uploadUrl, byte[] imageBytes);
        public Task<HttpResponseMessage> SharePostImageAsync(string imageId);
        public Task<HttpResponseMessage> InitializeDocument();
        public Task<HttpResponseMessage> UploadDocument(string uploadUrl, byte[] documentBytes);
        public Task<HttpResponseMessage> SharePostDocumentAsync(string documentId);
    }
}
