using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Files;
using Microsoft.AspNetCore.Identity;

namespace InternIntelligence_Portfolio.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>, IAuditable
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public UserProfilePictureFile? ProfilePictureFile { get; set; }
        public ICollection<Project> Projects { get; set; } = [];
        public ICollection<Skill> Skills { get; set; } = [];
        public ICollection<Achievement> Achievements { get; set; } = [];

        private ApplicationUser() { }
        private ApplicationUser(string firstName, string lastName, string username, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = username;
            Email = email;
        }

        public static ApplicationUser Create(string firstName, string lastName, string username, string email)
        {
            return new ApplicationUser(firstName, lastName, username, email);
        }
    }
}
