using InternIntelligence_Portfolio.Application.DTOs.Skill;

namespace InternIntelligence_Portfolio.Tests.Common.Factories
{
    public static partial class Factories
    {
        public static class Skills
        {
            public static IEnumerable<CreateSkillRequestDTO> GenerateMultipleValidCreateSkillRequestDTOs(byte count = 3)
            {
                for (int i = 1; i <= count; i++)
                {
                    yield return new CreateSkillRequestDTO
                    {
                        Name = $"{Constants.Constants.Skills.Name_Valid}-{i}",
                        Description = $"{Constants.Constants.Skills.Description_Valid}-{i}",
                        ProficiencyLevel = Constants.Constants.Skills.ProficiencyLevel_Valid,
                        YearsOfExperience = (byte)(Constants.Constants.Skills.YearsOfExperience_Valid + (byte)i),
                    };
                }
            }

            public static CreateSkillRequestDTO GenerateValidCreateSkillsRequestDTO()
            {
                return new CreateSkillRequestDTO
                {
                    Name = Constants.Constants.Skills.Name_Valid,
                    Description = Constants.Constants.Skills.Description_Valid,
                    ProficiencyLevel = Constants.Constants.Skills.ProficiencyLevel_Valid,
                    YearsOfExperience = Constants.Constants.Skills.YearsOfExperience_Valid,
                };
            }

            public static UpdateSkillRequestDTO GenerateValidUpdateSkillsRequestDTO()
            {
                return new UpdateSkillRequestDTO
                {
                    Name = Constants.Constants.Skills.Updated_Name_Valid,
                    Description = Constants.Constants.Skills.Updated_Description_Valid,
                    ProficiencyLevel = Constants.Constants.Skills.Updated_ProficiencyLevel_Valid,
                    YearsOfExperience = Constants.Constants.Skills.Updated_YearsOfExperience_Valid,
                };
            }

            public static CreateSkillRequestDTO GenerateInValidCreateSkillsRequestDTO()
            {
                return new CreateSkillRequestDTO
                {
                    Name = Constants.Constants.Skills.Name_InValid,
                    Description = Constants.Constants.Skills.Description_InValid,
                    ProficiencyLevel = Constants.Constants.Skills.ProficiencyLevel_InValid,
                    YearsOfExperience = Constants.Constants.Skills.YearsOfExperience_Valid,
                };
            }

            public static UpdateSkillRequestDTO GenerateInValidUpdateSkillsRequestDTO()
            {
                return new UpdateSkillRequestDTO
                {
                    Name = Constants.Constants.Skills.Name_InValid,
                    Description = Constants.Constants.Skills.Description_InValid,
                    ProficiencyLevel = Constants.Constants.Skills.ProficiencyLevel_InValid,
                    YearsOfExperience = Constants.Constants.Skills.YearsOfExperience_Valid,
                };
            }
        }
    }
}