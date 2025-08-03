using Application.Interfaces;
using Core.Exceptions;
using Core.Models;
using Persistence.Dto;

namespace Application.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly IShipmentResourceRepository _shipmentResourceRepository;
        private readonly IIncomeResourceRepository _incomeResourceRepository;

        public ResourceService(IResourceRepository resourceRepository, IShipmentResourceRepository shipmentResourceRepository, IIncomeResourceRepository incomeResourceRepository) 
        {
            _resourceRepository = resourceRepository;
            _shipmentResourceRepository = shipmentResourceRepository;
            _incomeResourceRepository = incomeResourceRepository;
        }

        public async Task<Resource> ChangeResourceStatusAsync(Guid id)
        {
            var resource = await _resourceRepository.GetResourceByIdAsync(id);
            resource.IsActive = !resource.IsActive;
            await _resourceRepository.UpdateResourceAsync(resource);
            return resource;
        }

        public async Task<Resource> AddResourceAsync(CreateResourceDto dto)
        {
            var resource = await _resourceRepository.GetResourceByNameAsync(dto.Name);
            if (resource != null)
            {
                throw new ConflictException($"Невозможно создать единицу измерения {dto.Name}. Такая уже зарегистрирована в системе");
            }

            resource = new Resource { Id =  dto.Id, Name = dto.Name };
            await _resourceRepository.AddResourceAsync(resource);
            return resource;
        }

        public async Task<Resource> UpdateResourceAsync(CreateResourceDto dto)
        {
            var resource = await _resourceRepository.GetResourceByIdAsync(dto.Id);

            if (resource.Name != dto.Name)
            {
                if (await _resourceRepository.GetResourceByNameAsync(dto.Name) != null)
                {
                    throw new ConflictException($"Невозможно создать единицу измерения {dto.Name}. Такая уже зарегистрирована в системе");
                }
            }

            var newResource = new Resource { Id = dto.Id, Name = dto.Name, IsActive = dto.IsActive };
            await _resourceRepository.UpdateResourceAsync(newResource);
            return newResource;
        }

        public async Task<Resource> DeleteResourceAsync(Guid id)
        {
            if(await CheckResourceToUse(id))
                throw new ConflictException($"Невозможно удалить единицу измерения, так как она используется в системе");

            var resource = await _resourceRepository.RemoveResourceAsync(id);
            return resource;
        }

        public async Task<bool> CheckResourceToUse( Guid id)
        {
            var shipmentsResource = await _shipmentResourceRepository.GetShipmentResourcesByResourceIdAsync(id);
            var incomeResource = await _incomeResourceRepository.GetIncomeResourcesByResourceIdAsync(id);

            return (shipmentsResource != null || incomeResource != null);
        }
    }
}
