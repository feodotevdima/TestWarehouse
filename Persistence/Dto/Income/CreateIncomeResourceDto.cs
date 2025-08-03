namespace Persistence.Dto
{
    public class CreateIncomeResourceDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ResourceId { get; set; }

        public Guid UnitId { get; set; }

        public decimal Quantity { get; set; }
    }
}
