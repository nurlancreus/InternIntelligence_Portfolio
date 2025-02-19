using InternIntelligence_Portfolio.Domain.Entities.Identity;
using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Domain.Entities.Files
{
    public class UserProfilePictureFile : ApplicationFile
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        private UserProfilePictureFile() { }
        private UserProfilePictureFile(string name, string path, StorageType storage) : base(name, path, storage) { }

        public static UserProfilePictureFile Create(string name, string path, StorageType storage)
        {
            return new UserProfilePictureFile(name, path, storage);
        }
    }
}
