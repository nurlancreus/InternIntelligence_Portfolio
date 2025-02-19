namespace InternIntelligence_Portfolio.Domain.Abstractions
{
    public abstract class Base : IAuditable
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
