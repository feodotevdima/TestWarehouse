using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TestWarehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceRepository _balanceRepository;

        public BalanceController(IBalanceRepository balanceRepository)
        {
            _balanceRepository = balanceRepository;
        }

        [HttpGet]
        public async Task<IResult> GetBalanceAsync([FromQuery] List<Guid>? resource_id = null, [FromQuery] List<Guid>? unit_id = null)
        {
            var balance = await _balanceRepository.GetFiltredBalanceAsync(resource_id, unit_id);
            return Results.Ok(balance);
        }
    }
}

