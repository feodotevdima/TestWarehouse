using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Persistence.Dto;

namespace TestWarehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentController(IShipmentRepository shipmentRepository, IShipmentService shipmentService)
        {
            _shipmentRepository = shipmentRepository;
            _shipmentService = shipmentService;
        }

        [HttpGet]
        public async Task<IResult> GetShipmentsAsync([FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null, [FromQuery] List<string>? number = null, [FromQuery] List<Guid>? resource_id = null, [FromQuery] List<Guid>? client_id = null, [FromQuery] List<Guid>? unit_id = null)
        {
            var shipments = await _shipmentRepository.GetFiltredShipmentDtosAsync(start, end, number, resource_id, client_id, unit_id);
            return Results.Ok(shipments);
        }

        [HttpGet("id/{id}")]
        public async Task<IResult> GetShipmentsByIdAsync(Guid id)
        {
            var shipment = await _shipmentRepository.GetShipmentDtoByIdAsync(id);
            return Results.Ok(shipment);
        }

        [HttpPost]
        public async Task<IResult> CreateShipmentsAsync([FromBody] CreateShipmentDto dto)
        {
            var shipment = await _shipmentService.AddShipmentAsync(dto);
            return Results.Ok(dto);
        }

        [HttpPut]
        public async Task<IResult> UpdateShipmentsAsync([FromBody] CreateShipmentDto dto)
        {
            var shipment = await _shipmentService.UpdateShipmentAsync(dto);
            return Results.Ok(dto);
        }

        [HttpPut("change-status/{id}")]
        public async Task<IResult> ChangeShipmentStatusAsync(Guid id)
        {
            var shipment = await _shipmentService.ChangeShipmentStatusAsync(id);
            return Results.Ok(shipment);
        }

        [HttpDelete]
        public async Task<IResult> DeleteShipmentsAsync([FromBody] Guid Id)
        {
            var shipment = await _shipmentService.DeleteShipmentAsync(Id);
            return Results.Ok(shipment);
        }
    } 
}

