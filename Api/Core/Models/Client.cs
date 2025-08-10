namespace Core.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Shipment> Shipments { get; set; }
    }
}
