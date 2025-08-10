using Core.Models;

namespace Application.Interfaces
{
    public interface IShipmentResourceRepository
    {
        Task<IEnumerable<ShipmentResource>?> GetShipmentResourcesByUnitIdAsync(Guid unitId);
        Task<IEnumerable<ShipmentResource>?> GetShipmentResourcesByResourceIdAsync(Guid resourceId);
        Task<IEnumerable<ShipmentResource>?> GetShipmentResourceByShipmentIdAsync(Guid shipmentId);
        Task<IEnumerable<Resource>?> GetResourcesUsedInShipmentAsync();
        Task<IEnumerable<Unit>?> GetUnitsUsedInShipmentAsync();
        Task<ShipmentResource> AddShipmentResourceAsync(ShipmentResource shipmentResource);
        Task<IEnumerable<ShipmentResource>> AddShipmentResourceAsync(IEnumerable<ShipmentResource> shipmentResource);
        Task<ShipmentResource> UpdateShipmentResourceAsync(ShipmentResource shipmentResource);
        Task<ShipmentResource> RemoveShipmentResourceAsync(Guid Id);
    }
}
