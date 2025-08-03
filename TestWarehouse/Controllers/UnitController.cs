using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Persistence.Dto;

namespace TestWarehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnitController : ControllerBase
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IUnitService _unitService;

        public UnitController(IUnitRepository unitRepository, IUnitService unitService)
        {
            _unitRepository = unitRepository;
            _unitService = unitService;
        }

        [HttpGet("all")]
        public async Task<IResult> GetAllUnitsAsync()
        {
            var units = await _unitRepository.GetAllUnitsAsync();
            return Results.Ok(units);
        }

        [HttpGet("active")]
        public async Task<IResult> GetActiveUnitsAsync()
        {
            var units = await _unitRepository.GetActiveUnitsAsync();
            return Results.Ok(units);
        }

        [HttpGet("archive")]
        public async Task<IResult> GetArchiveUnitsAsync()
        {
            var units = await _unitRepository.GetArchiveUnitsAsync();
            return Results.Ok(units);
        }

        [HttpGet("id/{id}")]
        public async Task<IResult> GetUnitByIdAsync(Guid id)
        {
            var unit = await _unitRepository.GetUnitByIdAsync(id);
            return Results.Ok(unit);
        }

        [HttpPost]
        public async Task<IResult> AddUnitAsync([FromBody] CreateUnitDto unit)
        {
            var newUnit = await _unitService.AddUnitAsync(unit);
            return Results.Ok(newUnit);
        }

        [HttpPut("change-status/{id}")]
        public async Task<IResult> ChangeUnitStatusAsync(Guid id)
        {
            var unit = await _unitService.ChangeUnitStatusAsync(id);
            return Results.Ok(unit);
        }

        [HttpPut]
        public async Task<IResult> UpdateUnitAsync([FromBody] CreateUnitDto unit)
        {
            var newUnit = await _unitService.UpdateUnitAsync(unit);
            return Results.Ok(newUnit);
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteUnitAsync(Guid id)
        {
            var newUnit = await _unitService.DeleteUnitAsync(id);
            return Results.Ok(newUnit);
        }
    }
}
