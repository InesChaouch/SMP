using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPost.Infrastructure.Settings
{
    public class LinkedInSettings
    {
        public required string AccessToken { get; set; }
        public required string UserUrn { get; set; }
    }
}
