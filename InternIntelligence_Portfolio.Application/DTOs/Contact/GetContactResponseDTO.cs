using ContactEntity = InternIntelligence_Portfolio.Domain.Entities.Contact;

namespace InternIntelligence_Portfolio.Application.DTOs.Contact
{
    public record GetContactResponseDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public GetContactResponseDTO(ContactEntity contact)
        {
            Id = contact.Id;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            Email = contact.Email;
            Message = contact.Message;
            CreatedAt = contact.CreatedAt;
            UpdatedAt = contact.UpdatedAt;
        }
    }
}
