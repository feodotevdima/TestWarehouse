namespace Persistence.Dto
{
    public class CreateIncomeDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Number { get; set; }
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public List<CreateIncomeResourceDto> resources { get; set; }
    }
}
