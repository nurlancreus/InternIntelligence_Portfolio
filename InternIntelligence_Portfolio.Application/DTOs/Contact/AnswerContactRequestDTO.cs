namespace InternIntelligence_Portfolio.Application.DTOs.Contact
{
    public record AnswerContactRequestDTO
    {
        public Guid ContactId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
