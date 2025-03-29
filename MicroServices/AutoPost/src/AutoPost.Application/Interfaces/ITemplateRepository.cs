using AutoPost.Domain.Entities.TemplateEntities;

namespace AutoPost.Application;
public interface ITemplateRepository
{
    public Task<IEnumerable<Template>> GetTemplatesAsync();

    public Task<Template?> GetTemplateByIdAsync(int id);
}
