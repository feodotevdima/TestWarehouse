namespace Persistence.Dto
{
    public class CreateIncomeDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Number { get; set; }
        public DateTime Date { get; set; }

        public List<CreateIncomeResourceDto> resources { get; set; }
    }
}
