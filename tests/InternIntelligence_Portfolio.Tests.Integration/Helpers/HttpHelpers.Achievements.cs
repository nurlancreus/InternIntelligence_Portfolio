using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace InternIntelligence_Portfolio.Tests.Integration.Helpers
{
    public static partial class HttpHelpers
    {
        public async static Task<Guid> CreateSingleAchievementAsync(this HttpClient client, IServiceScope scope)
        {
            var request = Factories.Achievements.GenerateValidCreateAchievementRequestDTO();

            var achievementId = await SendAsync(request, client, scope);

            return achievementId;

        }

        public async static Task<IEnumerable<Guid>> CreateMultipleAchievementsAsync(this HttpClient client, IServiceScope scope, byte count = 3)
        {
            var requestDtos = Factories.Achievements.GenerateMultipleValidCreateAchievementRequestDTOs(count);

            List<Guid> achievementIds = [];

            foreach (var requestDTO in requestDtos)
            {
                var achievementId = await SendAsync(requestDTO, client, scope);

                achievementIds.Add(achievementId);
            }

            return achievementIds;
        }

        private async static Task<Guid> SendAsync(CreateAchievementRequestDTO request, HttpClient client, IServiceScope scope)
        {
            var response = await client.SendRequestWithAccessToken(HttpMethod.Post, "api/achievements", scope, request);

            if (response.StatusCode is not HttpStatusCode.OK)
                throw new InvalidOperationException("Response is not successful");

            return await response.Content.ReadFromJsonAsync<Guid>();
        }
    }
}
