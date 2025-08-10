namespace Persistence.Dto
{
    public class CreateClientDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
