using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Decorators
{
    public class AchievementServiceValidationDecorator(IAchievementService innerAchievementService, RequestValidator requestValidator) : IAchievementService
    {
        private readonly IAchievementService _innerAchievementService = innerAchievementService;
        private readonly RequestValidator _requestValidator = requestValidator;

        public async Task<Result<Guid>> CreateAsync(CreateAchievementRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsFailure)
                return Result<Guid>.Failure(validationResult.Error);

            return await _innerAchievementService.CreateAsync(request, cancellationToken);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _innerAchievementService.DeleteAsync(id, cancellationToken);
        }

        public async Task<Result<IEnumerable<GetAchievementResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _innerAchievementService.GetAllAsync(cancellationToken);
        }

        public async Task<Result<GetAchievementResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _innerAchievementService.GetAsync(id, cancellationToken);
        }

        public async Task<Result<Guid>> UpdateAsync(UpdateAchievementRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsFailure)
                return Result<Guid>.Failure(validationResult.Error);

            return await _innerAchievementService.UpdateAsync(request, cancellationToken);
        }
    }
}
