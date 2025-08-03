namespace Core.Models
{
    public class IncomeResource
    {
        public Guid Id { get; set; }

        public Guid IncomeId { get; set; }
        public Income Income { get; set; }

        public Guid ResourceId { get; set; }
        public Resource Resource { get; set; }

        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }

        public decimal Quantity { get; set; }
    }
}
