namespace Core.Models
{
    public class Income
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }

        public ICollection<IncomeResource> IncomeResources { get; set; }
    }
}
