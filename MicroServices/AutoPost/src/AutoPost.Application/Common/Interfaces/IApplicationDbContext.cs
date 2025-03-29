using SMP.Domain.Entities;
using SMP.Domain.Entities.TemplateEntities;

namespace SMP.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<TemplateSlide> TemplateSlides { get; set; }
    public DbSet<TemplateSlideElement> TemplateSlideElements { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
