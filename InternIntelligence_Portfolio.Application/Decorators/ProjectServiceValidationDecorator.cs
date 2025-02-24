using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Project;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Decorators
{
    public class ProjectServiceValidationDecorator(IProjectService innerProjectService, RequestValidator requestValidator) : IProjectService
    {
        private readonly IProjectService _innerProjectService = innerProjectService;
        private readonly RequestValidator _requestValidator = requestValidator;

        public async Task<Result<Guid>> CreateAsync(CreateProjectRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsFailure)
                return Result<Guid>.Failure(validationResult.Error);

            return await _innerProjectService.CreateAsync(request, cancellationToken);
        }

        public Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _innerProjectService.DeleteAsync(id, cancellationToken);
        }

        public async Task<Result<IEnumerable<GetProjectResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _innerProjectService.GetAllAsync(cancellationToken);
        }

        public async Task<Result<GetProjectResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _innerProjectService.GetAsync(id, cancellationToken);
        }

        public async Task<Result<Guid>> UpdateAsync(UpdateProjectRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsFailure)
                return Result<Guid>.Failure(validationResult.Error);

            return await _innerProjectService.UpdateAsync(request, cancellationToken);
        }
    }
}
