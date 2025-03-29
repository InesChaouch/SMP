using AutoPost.Domain.Entities;
using AutoPost.Domain.Entities.PostEntities;
using AutoPost.Domain.Entities.TemplateEntities;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace AutoPost.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<TemplateSlide> TemplateSlides { get; set; }
    public DbSet<TemplateSlideElement> TemplateSlideElements { get; set; }
    public DbSet<Post> Posts { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync();

    // Ajout : gestion de transaction
    //Task<IDbContextTransaction> BeginTransactionAsync();
    //Task CommitTransactionAsync();
    //Task RollbackTransactionAsync();
}
