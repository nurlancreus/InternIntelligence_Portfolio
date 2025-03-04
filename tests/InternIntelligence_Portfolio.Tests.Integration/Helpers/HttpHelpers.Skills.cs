using InternIntelligence_Portfolio.Application.DTOs.Skill;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace InternIntelligence_Portfolio.Tests.Integration.Helpers
{
    public static partial class HttpHelpers
    {
        public async static Task<Guid> CreateSingleSkillAsync(this HttpClient client, IServiceScope scope)
        {
            var request = Factories.Skills.GenerateValidCreateSkillRequestDTO();

            var skillId = await SendAsync(request, client, scope);

            return skillId;

        }

        public async static Task<IEnumerable<Guid>> CreateMultipleSkillsAsync(this HttpClient client, IServiceScope scope, byte count = 3)
        {
            var requestDtos = Factories.Skills.GenerateMultipleValidCreateSkillRequestDTOs(count);

            List<Guid> skillIds = [];

            foreach (var requestDTO in requestDtos)
            {
                var skillId = await SendAsync(requestDTO, client, scope);

                skillIds.Add(skillId);
            }

            return skillIds;
        }

        private async static Task<Guid> SendAsync(CreateSkillRequestDTO request, HttpClient client, IServiceScope scope)
        {
            var response = await client.SendRequestWithAccessToken(HttpMethod.Post, "api/skills", scope, request);

            if (response.StatusCode is not HttpStatusCode.OK)
                throw new InvalidOperationException("Response is not successful");

            return await response.Content.ReadFromJsonAsync<Guid>();
        }
    }
}
