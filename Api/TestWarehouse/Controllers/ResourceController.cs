    using Application.Interfaces;
using Application.Repository;
using Microsoft.AspNetCore.Mvc;
using Persistence.Dto;

namespace TestWarehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceController : ControllerBase
    {

        private readonly IResourceRepository _resourceRepository;
        private readonly IResourceService _resourceService;
        private readonly IShipmentResourceRepository _shipmentResourceRepository;
        private readonly IIncomeResourceRepository _incomeResourceRepository;
        private readonly IBalanceRepository _balanceRepository;

        public ResourceController(IResourceRepository resourceRepository, IResourceService resourceService, IShipmentResourceRepository shipmentResourceRepository, IIncomeResourceRepository incomeResourceRepository, IBalanceRepository balanceRepository)
        {
            _resourceRepository = resourceRepository;
            _resourceService = resourceService;
            _shipmentResourceRepository = shipmentResourceRepository;
            _incomeResourceRepository = incomeResourceRepository;
            _balanceRepository = balanceRepository;
        }

        [HttpGet("all")]
        public async Task<IResult> GetAllResourcesAsync()
        {
            var resources = await _resourceRepository.GetAllResourcesAsync();
            return Results.Ok(resources);
        }

        [HttpGet("active")]
        public async Task<IResult> GetActiveResourcesAsync()
        {
            var resources = await _resourceRepository.GetActiveResourcesAsync();
            return Results.Ok(resources);
        }

        [HttpGet("archive")]
        public async Task<IResult> GetArchiveResourcesAsync()
        {
            var resources = await _resourceRepository.GetArchiveResourcesAsync();
            return Results.Ok(resources);
        }

        [HttpGet("id/{id}")]
        public async Task<IResult> GetResourceByIdAsync(Guid id)
        {
            var resource = await _resourceRepository.GetResourceByIdAsync(id);
            return Results.Ok(resource);
        }

        [HttpGet("use-in-shipments")]
        public async Task<IResult> GetResourcesUseInShipmentsAsync()
        {
            var resources = await _shipmentResourceRepository.GetResourcesUsedInShipmentAsync();
            return Results.Ok(resources);
        }

        [HttpGet("use-in-incomes")]
        public async Task<IResult> GetResourcesUseInIncomesAsync()
        {
            var resources = await _incomeResourceRepository.GetResourcesUsedInIncomeAsync();
            return Results.Ok(resources);
        }

        [HttpGet("use-in-balance")]
        public async Task<IResult> GetResourcesUseInBalanceAsync()
        {
            var resources = await _balanceRepository.GetResourcesUsedInBalanceAsync();
            return Results.Ok(resources);
        }

        [HttpPost]
        public async Task<IResult> AddResourceAsync([FromBody] CreateResourceDto resource)
        {
            var newResource = await _resourceService.AddResourceAsync(resource);
            return Results.Ok(newResource);
        }

        [HttpPut("change-status/{id}")]
        public async Task<IResult> ChangeResourceStatusAsync(Guid id)
        {
            var resource = await _resourceService.ChangeResourceStatusAsync(id);
            return Results.Ok(resource);
        }

        [HttpPut]
        public async Task<IResult> UpdateResourceAsync([FromBody] CreateResourceDto resource)
        {
            var newResource = await _resourceService.UpdateResourceAsync(resource);
            return Results.Ok(newResource);
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteResourceAsync(Guid id)
        {
            var newResource = await _resourceService.DeleteResourceAsync(id);
            return Results.Ok(newResource);
        }
    }
}
