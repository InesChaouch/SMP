using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPost.Domain.Enums;

namespace AutoPost.Application.PostContent.Commands.GenerateContent
{
        public class GenerateContentCommand : IRequest<string>
        {
            public required Platform Platform { get; set; }
            public Format Format { get; set; } = Format.Text;
            public required string Category { get; set; }
            public required string Topic { get; set; }
            public required List<string> Guidelines { get; set; }

        }
    }
