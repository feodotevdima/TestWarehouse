namespace Persistence.Dto
{
    public class CreateUnitDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
