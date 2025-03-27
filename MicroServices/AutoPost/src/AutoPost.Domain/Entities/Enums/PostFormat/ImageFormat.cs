using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMP.Domain.Entities.Enums.PostFormat
{
    public class ImageFormat : PostFormat
    {
        public required string ImageUrl { get; set; }

    }
}
