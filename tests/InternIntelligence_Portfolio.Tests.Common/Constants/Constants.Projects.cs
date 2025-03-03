using InternIntelligence_Portfolio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternIntelligence_Portfolio.Tests.Common.Constants
{
    public static partial class Constants
    {
        public static class Projects
        {
            public const string Name_Valid = "AI Portfolio";
            public const string Description_Valid = "An AI-powered portfolio showcasing projects.";
            public const string RepoUrl_Valid = "https://github.com/user/ai-portfolio";
            public const string LiveUrl_Valid = "https://aiportfolio.com";

            public const string Name_InValid = "";
            public const string Description_InValid = "";
            public const string RepoUrl_InValid = "invalid_url";
            public const string LiveUrl_InValid = "invalid_url";

            public const string Updated_Name_Valid = "Advanced AI Portfolio";
            public const string Updated_Description_Valid = "A more advanced AI-powered portfolio.";
            public const string Updated_RepoUrl_Valid = "https://github.com/user/advanced-ai-portfolio";
            public const string Updated_LiveUrl_Valid = "https://advancedaiportfolio.com";

            // Mock file details
            public const string FileName_Valid = "project_cover.png";
            public const string FileContentType_Valid = "image/png";

            public const string FileName_InValid = "document.pdf";
            public const string FileContentType_InValid = "application/pdf";

            public static readonly int MaxFileSizeBytes = DomainConstants.Project.ProjectCoverImageMaxSizeInMb * 1024 * 1024;
            public static readonly int OversizedFileSizeBytes = MaxFileSizeBytes + 1024 * 1024; // 1MB over limit
        }
    }
}
