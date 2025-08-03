namespace Persistence.Dto
{
    public class IncomeResourceDto
    {
        public Guid Id { get; set; }

        public Guid IncomeId { get; set; }

        public Guid ResourceId { get; set; }
        public string ResourceName { get; set; }

        public Guid UnitId { get; set; }
        public string UnitName { get; set; }

        public decimal Quantity { get; set; }
    }
}
