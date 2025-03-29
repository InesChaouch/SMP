using SMPlanner.Infrastructure.Enum;

namespace SMP.Application.PostContent.Commands.GenerateContent
{
    public class GenerateContentCommand : IRequest<string>
    {
        public required Platform Platform { get; set; }
        public Format Format { get; set; } = Format.Text;
        public required string Category { get; set; }
        public required string Topic { get; set; }
        public required List<string> Tones { get; set; }

        public required List<string> Guidelines { get; set; }

    }
}
