namespace Core.Models
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime Date { get; set; }
        public bool IsSigned {  get; set; } = false;

        public ICollection<ShipmentResource> ShipmentResources { get; set; }
    }
}
