using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Domain.Entities
{
    public class Contact : Base
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        private Contact() { }
        private Contact(string firstName, string lastName, string email, string message)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Message = message;
        }

        public static Contact Create(string firstName, string lastName, string email, string message)
        {
            return new Contact(firstName, lastName, email, message);
        }
    }
}
