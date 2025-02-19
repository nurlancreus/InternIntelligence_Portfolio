using InternIntelligence_Portfolio.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternIntelligence_Portfolio.Domain.Entities.Files
{
    public class ProjectCoverImageFile : ApplicationFile
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        private ProjectCoverImageFile() { }
        private ProjectCoverImageFile(string name, string path, StorageType storage) : base(name, path, storage) { }

        public static ProjectCoverImageFile Create(string name, string path, StorageType storage)
        {
            return new ProjectCoverImageFile(name, path, storage);
        }
    }
}
