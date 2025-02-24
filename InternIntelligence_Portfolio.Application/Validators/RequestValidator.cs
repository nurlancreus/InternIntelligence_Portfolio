using FluentValidation;
using InternIntelligence_Portfolio.Application.Abstractions;
using InternIntelligence_Portfolio.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace InternIntelligence_Portfolio.Application.Validators
{
    public class RequestValidator(IServiceScopeFactory serviceScopeFactory)
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        public async Task<Result<T>> ValidateAsync<T>(T request, CancellationToken cancellationToken = default) where T : IValidatableRequest
        {
            var requestType = request.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(requestType);

            using var scope = _serviceScopeFactory.CreateScope();

            var scopedProvider = scope.ServiceProvider;

            if (scopedProvider.GetService(validatorType) is not IValidator<T> validator)
                return Result<T>.Success(request);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return Result<T>.Failure(Error.ValidationError("Validation error occurred", validationErrors));
            }

            return Result<T>.Success(request);
        }
    }
}
