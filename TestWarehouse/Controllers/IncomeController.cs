using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Persistence.Dto;

namespace TestWarehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeRepository incomeRepository, IIncomeService incomeService)
        {
            _incomeRepository = incomeRepository;
            _incomeService = incomeService;
        }

        [HttpGet]
        public async Task<IResult> GetIncomesAsync([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null, [FromQuery] List<string> number = null, [FromQuery] List<Guid>? resource_id = null, [FromQuery] List<Guid>? unit_id = null)
        {
            var income = await _incomeRepository.GetFiltredIncomeDtosAsync(start, end, number, resource_id, unit_id);
            return Results.Ok(income);
        }

        [HttpGet("id/{id}")]
        public async Task<IResult> GetIncomeByIdAsync(Guid id)
        {
            var income = await _incomeRepository.GetIncomeByIdAsync(id);
            return Results.Ok(income);
        }

        [HttpPost]
        public async Task<IResult> CreateIncomeAsync([FromBody]CreateIncomeDto dto)
        {
            var income = await _incomeService.AddIncomeAsync(dto);
            return Results.Ok(income);
        }

        [HttpPut]
        public async Task<IResult> UpdateIncomeAsync([FromBody] CreateIncomeDto dto)
        {
            var income = await _incomeService.UpdateIncomeAsync(dto);
            return Results.Ok(income);
        }

        [HttpDelete]
        public async Task<IResult> DeleteIncomeAsync([FromBody] Guid Id)
        {
            var income = await _incomeService.DeleteIncomeAsync(Id);
            return Results.Ok(income);
        }
    } 
}

