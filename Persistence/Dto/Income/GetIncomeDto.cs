namespace Persistence.Dto
{
    public class GetIncomeDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }

        public List<IncomeResourceDto> resources { get; set; }
    }
}
