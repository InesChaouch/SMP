using AutoPost.Domain.Entities.PostEntities;

namespace AutoPost.Application;
public interface IPostRepository
{
    public Task<IEnumerable<Post>> GetPostsAsync();

    public  Task<Post?> GetPostByIdAsync(int id);

    public Task AddPostAsync(Post post);

    public void UpdatePost(Post post);
}
