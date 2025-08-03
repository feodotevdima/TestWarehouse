namespace Persistence.Dto
{
    public class GetShipmentDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public bool IsSigned { get; set; }

        public List<ShipmentResourceDto> resources { get; set; }
    }
}
