namespace Core.Models
{
    public class Resource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Balance> Balances { get; set; }
        public ICollection<IncomeResource> IncomeResources { get; set; }
        public ICollection<ShipmentResource> ShipmentResources { get; set; }
    }
}
