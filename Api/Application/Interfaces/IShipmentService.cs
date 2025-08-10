using Core.Models;
using Persistence.Dto;

namespace Application.Interfaces
{
    public interface IShipmentService
    {
        Task<Shipment> AddShipmentAsync(CreateShipmentDto dto);
        Task<Shipment> ChangeShipmentStatusAsync(Guid id);
        Task<Shipment> UpdateShipmentAsync(CreateShipmentDto dto);
        Task<Shipment> DeleteShipmentAsync(Guid id);
    }
}
