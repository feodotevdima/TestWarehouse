using Core.Models;
using Persistence.Dto;

namespace Application.Interfaces
{
    public interface IResourceService
    {
        Task<Resource> ChangeResourceStatusAsync(Guid id);
        Task<Resource> AddResourceAsync(CreateResourceDto dto);
        Task<Resource> UpdateResourceAsync(CreateResourceDto dto);
        Task<Resource> DeleteResourceAsync(Guid id);
    }
}
