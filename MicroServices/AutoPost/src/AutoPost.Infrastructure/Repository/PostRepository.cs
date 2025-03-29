using AutoPost.Application;
using AutoPost.Application.Common.Interfaces;
using AutoPost.Domain.Entities.PostEntities;
using Microsoft.EntityFrameworkCore;

namespace AutoPost.Infrastructure.Repository;
public class PostRepository : IPostRepository
{
    private readonly IApplicationDbContext _context;

    public PostRepository(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        return await _context.Posts
            //.Include(x => x.ChannelProfile)
            //.Include(x => x.PostFormat)
            //.Include(x => x.PostStatus)
            //.Include(x => x.Template)
            .ToListAsync();

    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _context.Posts.FindAsync(id);

    }

    public async Task AddPostAsync(Post post)
    {
       await _context.Posts.AddAsync(post);

    }

    public void UpdatePost(Post post)
    {
        _context.Posts.Update(post);
    }
}
