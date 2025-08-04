using Core.Models;
using Persistence.Dto;

namespace Application.Interfaces
{
    public interface IShipmentRepository
    {
        Task<IEnumerable<GetShipmentDto>> GetFiltredShipmentDtosAsync(DateOnly? start = null, DateOnly? end = null, List<string>? numbers = null, List<Guid>? clientIds = null, List<Guid>? resourceIds = null, List<Guid>? unitIds = null);
        Task<GetShipmentDto?> GetShipmentDtoByIdAsync(Guid id);
        Task<Shipment?> GetShipmentByIdAsync(Guid id);
        Task<IEnumerable<Shipment>?> GetShipmentsByClientIdAsync(Guid clientId);
        Task<Shipment> AddShipmentAsync(Shipment shipment);
        Task<Shipment> UpdateShipmentAsync(Shipment shipment);
        Task<Shipment> RemoveShipmentAsync(Guid Id);
    }
}
