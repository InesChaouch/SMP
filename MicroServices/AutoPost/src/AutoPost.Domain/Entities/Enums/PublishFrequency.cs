using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPost.Domain.Entities.Enums
{
    public class PublishFrequency
    {
        public int Id { get; set; }
        public required string Description { get; set; }
    }
}
