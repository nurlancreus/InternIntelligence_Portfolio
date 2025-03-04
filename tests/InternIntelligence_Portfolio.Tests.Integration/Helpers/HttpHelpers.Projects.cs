using Microsoft.Extensions.DependencyInjection;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using System.Net;
using System.Net.Http.Json;
using InternIntelligence_Portfolio.Application.DTOs.Project;

namespace InternIntelligence_Portfolio.Tests.Integration.Helpers
{
    public static partial class HttpHelpers
    {
        public async static Task<Guid> CreateSingleProjectAsync(this HttpClient client, IServiceScope scope)
        {
            var request = Factories.Projects.GenerateValidCreateProjectRequestDTO();

            var projectId = await SendAsync(request, client, scope);

            return projectId;

        }

        public async static Task<IEnumerable<Guid>> CreateMultipleProjectsAsync(this HttpClient client, IServiceScope scope, byte count = 3, bool includeFile = false)
        {
            var requestDtos = Factories.Projects.GenerateMultipleValidCreateProjectRequestDTOs(count, includeFile);

            List<Guid> projectIds = [];

            foreach (var requestDTO in requestDtos)
            {
                var projectId = await SendAsync(requestDTO, client, scope);

                projectIds.Add(projectId);
            }

            return projectIds;
        }

        private async static Task<Guid> SendAsync(CreateProjectRequestDTO request, HttpClient client, IServiceScope scope)
        {
            var response = await client.SendRequestWithAccessToken(HttpMethod.Post, "api/projects", scope, request, isFromForm: true);

            if (response.StatusCode is not HttpStatusCode.OK)
                throw new InvalidOperationException("Response is not successful");

            return await response.Content.ReadFromJsonAsync<Guid>();
        }
    }
}
