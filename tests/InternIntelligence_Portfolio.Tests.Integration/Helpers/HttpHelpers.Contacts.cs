using Microsoft.Extensions.DependencyInjection;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using System.Net;
using System.Net.Http.Json;
using InternIntelligence_Portfolio.Application.DTOs.Contact;

namespace InternIntelligence_Portfolio.Tests.Integration.Helpers
{
    public static partial class HttpHelpers
    {
        public async static Task<Guid> CreateSingleContactAsync(this HttpClient client, IServiceScope scope)
        {
            var request = Factories.Contacts.GenerateValidCreateContactRequestDTO();

            var contactId = await SendAsync(request, client, scope);

            return contactId;
        }

        public async static Task<IEnumerable<Guid>> CreateMultipleContactsAsync(this HttpClient client, IServiceScope scope, byte count = 3)
        {
            var requestDtos = Factories.Contacts.GenerateMultipleValidCreateContactRequestDTOs(count);

            List<Guid> contactIds = [];

            foreach (var requestDTO in requestDtos)
            {
                var contactId = await SendAsync(requestDTO, client, scope);

                contactIds.Add(contactId);
            }

            return contactIds;
        }

        private async static Task<Guid> SendAsync(CreateContactRequestDTO request, HttpClient client, IServiceScope scope)
        {
            var response = await client.SendRequestWithAccessToken(HttpMethod.Post, "api/contacts", scope, request);

            if (response.StatusCode is not HttpStatusCode.OK)
                throw new InvalidOperationException("Response is not successful");

            return await response.Content.ReadFromJsonAsync<Guid>();
        }
    }
}
