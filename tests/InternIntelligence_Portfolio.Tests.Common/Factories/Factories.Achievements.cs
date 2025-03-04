using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Services;

namespace InternIntelligence_Portfolio.Tests.Common.Factories
{
    public static partial class Factories
    {
        public static class Achievements
        {
            public static IEnumerable<CreateAchievementRequestDTO> GenerateMultipleValidCreateAchievementRequestDTOs(byte count = 3)
            {
                for (int i = 1; i <= count; i++)
                {
                    yield return new CreateAchievementRequestDTO
                    {
                        Title = $"{Constants.Constants.Achievements.Title_Valid}-{i}",
                        Description = $"{Constants.Constants.Achievements.Description_Valid}-{i}",
                        AchievedAt = Constants.Constants.Achievements.AchievedAt_Valid,
                    };
                }
            }

            public static CreateAchievementRequestDTO GenerateValidCreateAchievementRequestDTO()
            {
                return new CreateAchievementRequestDTO
                {
                    Title = Constants.Constants.Achievements.Title_Valid,
                    Description = Constants.Constants.Achievements.Description_Valid,
                    AchievedAt = Constants.Constants.Achievements.AchievedAt_Valid,
                };
            }

            public static UpdateAchievementRequestDTO GenerateValidUpdateAchievementRequestDTO()
            {
                return new UpdateAchievementRequestDTO
                {
                    Title = Constants.Constants.Achievements.Updated_Title_Valid,
                    Description = Constants.Constants.Achievements.Updated_Description_Valid,
                    AchievedAt = Constants.Constants.Achievements.Updated_AchievedAt_Valid,
                };
            }

            public static CreateAchievementRequestDTO GenerateInValidCreateAchievementRequestDTO()
            {
                return new CreateAchievementRequestDTO
                {
                    Title = Constants.Constants.Achievements.Title_InValid,
                    Description = Constants.Constants.Achievements.Description_InValid,
                    AchievedAt = Constants.Constants.Achievements.AchievedAt_InValid,
                };
            }

            public static UpdateAchievementRequestDTO GenerateInValidUpdateAchievementRequestDTO()
            {
                return new UpdateAchievementRequestDTO
                {
                    Title = Constants.Constants.Achievements.Title_InValid,
                    Description = Constants.Constants.Achievements.Description_InValid,
                    AchievedAt = Constants.Constants.Achievements.AchievedAt_InValid,
                };
            }
        }
    }
}
