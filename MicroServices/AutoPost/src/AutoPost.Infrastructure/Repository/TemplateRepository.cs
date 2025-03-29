using System.Threading;
using AutoPost.Application;
using AutoPost.Application.Common.Interfaces;
using AutoPost.Domain.Entities.TemplateEntities;
using Microsoft.EntityFrameworkCore;

namespace SMP.Infrastructure.Repository;
internal class TemplateRepository :ITemplateRepository
{
    public readonly IApplicationDbContext _context;
    public TemplateRepository(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Template>> GetTemplatesAsync()
    {
        return await _context.Templates
                   .ToListAsync();
    }

    public async Task<Template?> GetTemplateByIdAsync(int id)
    {
       return await _context.Templates
                    .Include(t => t.TemplateSlides)
                    .ThenInclude(s => s.Elements)
                    .SingleOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Template?> GetTemplateByFormatAsync(string format)
    {
        return await _context.Templates
                     .Include(t => t.TemplateSlides)
                     .ThenInclude(s => s.Elements)
                     .SingleOrDefaultAsync(t => t.Format == format);
    }
}
