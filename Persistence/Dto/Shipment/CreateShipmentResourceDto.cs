namespace Persistence.Dto
{
    public class CreateShipmentResourceDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ResourceId { get; set; }

        public Guid UnitId { get; set; }

        public decimal Quantity { get; set; }
    }
}
