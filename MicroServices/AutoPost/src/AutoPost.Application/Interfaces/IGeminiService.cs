using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPost.Application.Interfaces
{
    public interface IGeminiService
    {
        public Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken);
    }
}
