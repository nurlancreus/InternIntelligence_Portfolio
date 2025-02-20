using InternIntelligence_Portfolio.Application.DTOs.Project;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services
{
    public interface IProjectService
    {
        Task<Result<IEnumerable<GetProjectResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<GetProjectResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<Guid>> CreateAsync(CreateProjectRequestDTO createProjectRequest, CancellationToken cancellationToken = default);
        Task<Result<Guid>> UpdateAsync(UpdateProjectRequestDTO updateProjectRequest, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
