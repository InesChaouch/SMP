using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMP.Domain.Entities.PostEntities
{
    public class LinkedInPost : Post
    {
        public required string Visibility { get; set; }
    }
}
