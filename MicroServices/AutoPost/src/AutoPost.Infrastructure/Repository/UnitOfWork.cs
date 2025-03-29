using AutoPost.Application;
using AutoPost.Application.Common.Interfaces;
using AutoPost.Application.Interfaces;

namespace AutoPost.Infrastructure.Repository;
internal class UnitOfWork : IUnitOfWork
{
    private readonly IApplicationDbContext _context;
    public IPostRepository PostRepository { get; }
    public ITemplateRepository TemplateRepository { get; }

    public UnitOfWork(IApplicationDbContext context, IPostRepository postRepository, ITemplateRepository templateRepository)
    {
        _context = context;
        PostRepository = postRepository;
        TemplateRepository = templateRepository;
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    //public Task BeginTransactionAsync() => _context.BeginTransactionAsync();
    //public Task CommitTransactionAsync() => _context.CommitTransactionAsync();
    //public Task RollbackTransactionAsync() => _context.RollbackTransactionAsync();
}
