using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMP.Domain.Entities.PostEntities;

namespace SMP.Domain.Entities.Enums.PostFormat
{
    public abstract class PostFormat
    {
        public int Id { get; set; }
        public required string Description { get; set; }
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    }
}
