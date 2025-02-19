using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Enums;

namespace InternIntelligence_Portfolio.Domain.Entities.Files
{
    public class ApplicationFile : Base
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public StorageType Storage { get; set; }

        public string? Type { get; set; }
        protected ApplicationFile() { }
        protected ApplicationFile(string name, string path, StorageType storage)
        {
            Name = name;
            Path = path;
            Storage = storage;
        }
    }
}
