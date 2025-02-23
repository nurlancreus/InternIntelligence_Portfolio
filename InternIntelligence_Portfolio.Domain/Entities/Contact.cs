using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Domain.Entities
{
    public class Contact : Base
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsAnswered { get; set; }
        public ContactSubject Subject { get; set; }

        private Contact() { }
        private Contact(string firstName, string lastName, string email, string message, ContactSubject contactSubject)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Message = message;
            Subject = contactSubject;
            IsAnswered = false;
        }

        public static Contact Create(string firstName, string lastName, string email, string message, ContactSubject contactSubject)
        {
            return new Contact(firstName, lastName, email, message, contactSubject);
        }
    }
}
