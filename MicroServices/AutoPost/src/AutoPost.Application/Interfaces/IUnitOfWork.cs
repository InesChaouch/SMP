
namespace AutoPost.Application.Interfaces;
public interface IUnitOfWork
{
    public IPostRepository PostRepository { get; }
    public ITemplateRepository TemplateRepository { get; }
    Task<int> SaveChangesAsync();
    //Task BeginTransactionAsync();
    //Task CommitTransactionAsync();
    //Task RollbackTransactionAsync();
}
