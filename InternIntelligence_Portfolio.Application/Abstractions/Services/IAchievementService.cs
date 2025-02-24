using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services
{
    public interface IAchievementService
    {
        Task<Result<IEnumerable<GetAchievementResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<GetAchievementResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<Guid>> CreateAsync(CreateAchievementRequestDTO createAchievementRequest, CancellationToken cancellationToken = default);
        Task<Result<Guid>> UpdateAsync(Guid id, UpdateAchievementRequestDTO updateAchievementRequest, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
