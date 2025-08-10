namespace Core.Models
{
    public class ShipmentResource
    {
        public Guid Id { get; set; }

        public Guid ShipmentId { get; set; }
        public Shipment Shipment { get; set; }

        public Guid ResourceId { get; set; }
        public Resource Resource { get; set; }

        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }

        public decimal Quantity { get; set; }
    }
}
