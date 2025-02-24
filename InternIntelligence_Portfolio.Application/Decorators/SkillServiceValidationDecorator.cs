using InternIntelligence_Portfolio.Application.Abstractions.Services;
using InternIntelligence_Portfolio.Application.DTOs.Skill;
using InternIntelligence_Portfolio.Application.Validators;
using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Decorators
{
    public class SkillServiceValidationDecorator(ISkillService innerSkillService, RequestValidator requestValidator) : ISkillService
    {
        private readonly ISkillService _innerSkillService = innerSkillService;
        private readonly RequestValidator _requestValidator = requestValidator;

        public async Task<Result<Guid>> CreateAsync(CreateSkillRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsFailure)
                return Result<Guid>.Failure(validationResult.Error);

            return await _innerSkillService.CreateAsync(request, cancellationToken);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _innerSkillService.DeleteAsync(id, cancellationToken);

        }

        public async Task<Result<IEnumerable<GetSkillResponseDTO>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _innerSkillService.GetAllAsync(cancellationToken);

        }

        public async Task<Result<GetSkillResponseDTO>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _innerSkillService.GetAsync(id, cancellationToken);

        }

        public async Task<Result<Guid>> UpdateAsync(UpdateSkillRequestDTO request, CancellationToken cancellationToken = default)
        {
            var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsFailure)
                return Result<Guid>.Failure(validationResult.Error);

            return await _innerSkillService.UpdateAsync(request, cancellationToken);
        }
    }
}
