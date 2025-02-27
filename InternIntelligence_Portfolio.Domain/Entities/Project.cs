﻿using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Files;
using InternIntelligence_Portfolio.Domain.Entities.Identity;

namespace InternIntelligence_Portfolio.Domain.Entities
{
    public class Project : Base
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RepoUrl { get; set; } = string.Empty;
        public string LiveUrl { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public ProjectCoverImageFile? CoverImageFile { get; set; }

        private Project() { }
        private Project(string name, string description, string repoUrl, string liveUrl)
        {
            Name = name;
            Description = description;
            RepoUrl = repoUrl;
            LiveUrl = liveUrl;
        }

        public static Project Create(string name, string description, string repoUrl, string liveUrl)
        {
            return new Project(name, description, repoUrl, liveUrl);
        }
    }
}
