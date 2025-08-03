using Application.Interfaces;
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

        public ResourceController(IResourceRepository resourceRepository, IResourceService resourceService)
        {
            _resourceRepository = resourceRepository;
            _resourceService = resourceService;
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
