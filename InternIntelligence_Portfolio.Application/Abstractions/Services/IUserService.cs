using InternIntelligence_Portfolio.Application.DTOs.User;
using InternIntelligence_Portfolio.Domain.Abstractions;
using InternIntelligence_Portfolio.Domain.Entities.Identity;

namespace InternIntelligence_Portfolio.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<Result<GetUserResponseDTO>> GetMeAsync(CancellationToken cancellationToken = default);
        Task<Result<bool>> ChangeProfilePictureAsync(ChangeProfilePictureRequestDTO changeProfilePictureRequest, CancellationToken cancellationToken = default);
        Task<Result<bool>> UpdateUserRefreshToken(ApplicationUser user, string refreshToken, DateTime accessTokenEndDate);
    }
}
