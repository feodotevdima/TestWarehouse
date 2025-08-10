using Core.Models;

namespace Application.Interfaces
{
    public interface IResourceRepository
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync();
        Task<IEnumerable<Resource>> GetActiveResourcesAsync();
        Task<IEnumerable<Resource>> GetArchiveResourcesAsync();
        Task<Resource?> GetResourceByIdAsync(Guid id);
        Task<Resource?> GetResourceByNameAsync(string name);
        Task<Resource> AddResourceAsync(Resource resource);
        Task<Resource> UpdateResourceAsync(Resource resource);
        Task<Resource> RemoveResourceAsync(Guid Id);
    }
}
