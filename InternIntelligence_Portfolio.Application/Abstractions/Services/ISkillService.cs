using InternIntelligence_Portfolio.Application.DTOs.Skill;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services
{
    public interface ISkillService
    {
        Task<Result<IEnumerable<GetSkillResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<GetSkillResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<Guid>> CreateAsync(CreateSkillRequestDTO createSkillRequest, CancellationToken cancellationToken = default);
        Task<Result<Guid>> UpdateAsync(UpdateSkillRequestDTO updateSkillRequest, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
