namespace Persistence.Dto
{
    public class CreateShipmentDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Number { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public Guid ClientId { get; set; }
        public bool IsSigned { get; set; }

        public List<CreateShipmentResourceDto> resources { get; set; }
    }
}
