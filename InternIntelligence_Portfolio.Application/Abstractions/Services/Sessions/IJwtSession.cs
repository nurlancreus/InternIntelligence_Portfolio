using InternIntelligence_Portfolio.Domain.Abstractions;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services.Sessions
{
    public interface IJwtSession
    {
        Result<bool> ValidateIfSuperAdmin();
        Result<Guid> GetUserId();
    }
}
