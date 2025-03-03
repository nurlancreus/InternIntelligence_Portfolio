using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using InternIntelligence_Portfolio.Application.DTOs.File;
using InternIntelligence_Portfolio.Application.DTOs.Project;
using InternIntelligence_Portfolio.Application.DTOs.Skill;
using InternIntelligence_Portfolio.Domain.Entities.Identity;

namespace InternIntelligence_Portfolio.Application.DTOs.User
{
    public record GetUserResponseDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public GetImageFileResponseDTO? ProfilePictureFile { get; set; }
        public IEnumerable<GetProjectResponseDTO> Projects { get; set; } = [];
        public IEnumerable<GetSkillResponseDTO> Skills { get; set; } = [];
        public IEnumerable<GetAchievementResponseDTO> Achievements { get; set; } = [];

        public GetUserResponseDTO() { }
        public GetUserResponseDTO(ApplicationUser user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName!;
            Email = user.Email!;
            Bio = user.Bio;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;

            ProfilePictureFile = user.ProfilePictureFile is null ? null : new GetImageFileResponseDTO(user.ProfilePictureFile);

            Projects = user.Projects.Select(p => new GetProjectResponseDTO(p));
            Skills = user.Skills.Select(s => new GetSkillResponseDTO(s));
            Achievements = user.Achievements.Select(a => new GetAchievementResponseDTO(a));
        }
    }
}
