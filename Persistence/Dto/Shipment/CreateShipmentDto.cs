namespace Persistence.Dto
{
    public class CreateShipmentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public Guid ClientId { get; set; }
        public bool IsSigned { get; set; }

        public List<CreateIncomeResourceDto> resources { get; set; }
    }
}
