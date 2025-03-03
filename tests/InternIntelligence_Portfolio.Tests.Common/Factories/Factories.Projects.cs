using InternIntelligence_Portfolio.Application.DTOs.Project;

namespace InternIntelligence_Portfolio.Tests.Common.Factories
{
    public static partial class Factories
    {
        public static class Projects
        {
            public static IEnumerable<CreateProjectRequestDTO> GenerateMultipleValidCreateProjectRequestDTOs(byte count = 3, bool includeFile = false)
            {
                for (int i = 1; i <= count; i++)
                {

                   yield return new CreateProjectRequestDTO
                    {
                        Name = $"{Constants.Constants.Projects.Name_Valid}-{i}",
                        Description = $"{Constants.Constants.Projects.Description_Valid}-{i}",
                        RepoUrl = Constants.Constants.Projects.RepoUrl_Valid,
                        LiveUrl = Constants.Constants.Projects.LiveUrl_Valid,

                        ProjectCoverImageFile = includeFile
                            ? Utilities.GenerateMockFile(Constants.Constants.Projects.FileName_Valid, Constants.Constants.Projects.FileContentType_Valid, Constants.Constants.Projects.MaxFileSizeBytes)
                            : null
                    };
                }
            }

            public static CreateProjectRequestDTO GenerateValidCreateProjectRequestDTO(bool includeFile = false)
            {
                return new CreateProjectRequestDTO
                {
                    Name = Constants.Constants.Projects.Name_Valid,
                    Description = Constants.Constants.Projects.Description_Valid,
                    RepoUrl = Constants.Constants.Projects.RepoUrl_Valid,
                    LiveUrl = Constants.Constants.Projects.LiveUrl_Valid,

                    ProjectCoverImageFile = includeFile
                        ? Utilities.GenerateMockFile(Constants.Constants.Projects.FileName_Valid, Constants.Constants.Projects.FileContentType_Valid, Constants.Constants.Projects.MaxFileSizeBytes)
                        : null
                };
            }

            public static CreateProjectRequestDTO GenerateInValidCreateProjectRequestDTO(bool includeFile = false, bool oversized = false)
            {
                return new CreateProjectRequestDTO
                {
                    Name = Constants.Constants.Projects.Name_InValid,
                    Description = Constants.Constants.Projects.Description_InValid,
                    RepoUrl = Constants.Constants.Projects.RepoUrl_InValid,
                    LiveUrl = Constants.Constants.Projects.LiveUrl_InValid,
                    ProjectCoverImageFile = includeFile
                        ? Utilities.GenerateMockFile(
                            Constants.Constants.Projects.FileName_InValid,
                            Constants.Constants.Projects.FileContentType_InValid,
                            oversized ? Constants.Constants.Projects.OversizedFileSizeBytes : Constants.Constants.Projects.MaxFileSizeBytes
                          )
                        : null
                };
            }

            public static UpdateProjectRequestDTO GenerateValidUpdatedProjectRequestDTO(bool includeFile = false)
            {
                return new UpdateProjectRequestDTO
                {
                    Name = Constants.Constants.Projects.Updated_Name_Valid,
                    Description = Constants.Constants.Projects.Updated_Description_Valid,
                    RepoUrl = Constants.Constants.Projects.Updated_RepoUrl_Valid,
                    LiveUrl = Constants.Constants.Projects.Updated_LiveUrl_Valid,
                    ProjectCoverImageFile = includeFile
                        ? Utilities.GenerateMockFile(Constants.Constants.Projects.FileName_Valid, Constants.Constants.Projects.FileContentType_Valid, Constants.Constants.Projects.MaxFileSizeBytes)
                        : null
                };
            }
        }
    }
}
