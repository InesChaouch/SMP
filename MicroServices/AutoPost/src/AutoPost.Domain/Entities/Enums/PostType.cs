using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMP.Domain.Entities.Enums
{
    public class PostType
    {
        public int Id { get; set; }
        public required string Description { get; set; }
    }
}
