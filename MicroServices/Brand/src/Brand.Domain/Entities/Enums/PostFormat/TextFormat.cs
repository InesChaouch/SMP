using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPost.Domain.Entities.Enums.PostFormat
{
    public class TextFormat : PostFormat
    {
        public required string Text { get; set; }

    }
}
