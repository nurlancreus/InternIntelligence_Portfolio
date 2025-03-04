using FluentAssertions;
using InternIntelligence_Portfolio.Application.DTOs.Contact;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using InternIntelligence_Portfolio.Tests.Integration.Helpers;
using Microsoft.Extensions.DependencyInjection;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using System.Net;
using System.Net.Http.Json;

namespace InternIntelligence_Portfolio.Tests.Integration.Endpoints
{
    [Collection("Sequential")]
    public class ContactsEndpointTests : IClassFixture<TestingWebApplicationFactory>, IAsyncLifetime
    {
        private readonly TestingWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly AppDbContext _context;

        public ContactsEndpointTests(TestingWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _scope = _factory.Services.CreateScope();

            _context = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        public async Task InitializeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            _context.Dispose();
            _scope.Dispose();
            _client.Dispose();
        }
        // Valid tests

        [Fact]
        public async Task GetAllAsync_WhenProvidingValidRequest_ShouldReturnContacts()
        {
            // Arrange
            const byte contactsCount = 1;
            var ids = await _client.CreateMultipleContactsAsync(_scope, contactsCount);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/contacts", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var contacts = await response.Content.ReadFromJsonAsync<IEnumerable<GetContactResponseDTO>>();

            contacts.Should().NotBeEmpty();
            contacts.Count().Should().Be(contactsCount);
            contacts.All(contact => ids.Contains(contact.Id)).Should().BeTrue();
        }

        [Fact]
        public async Task GetAsync_WhenProvidingValidRequest_ShouldReturnContact()
        {
            // Arrange
            var id = await _client.CreateSingleContactAsync(_scope);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/contacts/{id}", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var contact = await response.Content.ReadFromJsonAsync<GetContactResponseDTO>();

            contact.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAsync_WhenProvidingValidRequest_ShouldReturnId()
        {
            // Arrange
            var request = Factories.Contacts.GenerateValidCreateContactRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Post, "api/contacts", _scope, request);

            // Assert
            response.EnsureSuccessStatusCode();
            var contactId = await response.Content.ReadFromJsonAsync<Guid>();
            contactId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task AnswerAsync_WhenProvidingValidRequest_ShouldReturnTrue()
        {
            // Arrange
            var request = Factories.Contacts.GenerateValidAnswerContactRequestDTO();
            var id = await _client.CreateSingleContactAsync(_scope);
            var accessToken = await _client.GetSuperAdminAccessTokenAsync(_scope);
            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, $"api/contacts/{id}/answer", _scope, request, accessToken: accessToken);

            // Assert
            response.EnsureSuccessStatusCode();
            var isSuccess = await response.Content.ReadFromJsonAsync<bool>();
            isSuccess.Should().BeTrue();

            var getResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/contacts/{id}", _scope, accessToken: accessToken);
            var contact = await getResponse.Content.ReadFromJsonAsync<GetContactResponseDTO>();

            contact.Should().NotBeNull();
            contact.IsAnswered.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_WhenProvidingValidRequest_ShouldReturnTrue()
        {
            // Arrange
            const byte contactsCount = 1;
            const byte countAfterDelete = contactsCount - 1;
            var ids = await _client.CreateMultipleContactsAsync(_scope, contactsCount);

            var id = ids.First();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Delete, $"api/contacts/{id}", _scope);

            // Assert
            response.EnsureSuccessStatusCode();

            var getAllResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/contacts", _scope);

            var contacts = await getAllResponse.Content.ReadFromJsonAsync<IEnumerable<GetContactResponseDTO>>();

            contacts.Should().NotBeEmpty();
            contacts.Count().Should().Be(countAfterDelete);
            contacts.Select(contact => contact.Id).Contains(id).Should().BeFalse();
        }

        // Invalid tests
        [Fact]
        public async Task GetAllAsync_WhenProvidingExtraRequest_ShouldReturnWrongCount()
        {
            // Arrange
            const byte contactsCount = 1;
            const byte contactsCountIfExtraEntityExist = contactsCount + 1;

            var id = await _client.CreateSingleContactAsync(_scope);
            var ids = await _client.CreateMultipleContactsAsync(_scope, contactsCount);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/contacts", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var contacts = await response.Content.ReadFromJsonAsync<IEnumerable<GetContactResponseDTO>>();

            contacts.Should().NotBeEmpty();
            contacts.Count().Should().Be(contactsCountIfExtraEntityExist);
            contacts.FirstOrDefault(contact => !ids.Contains(contact.Id))?.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetAsync_WhenProvidingInValidRequest_ShouldReturnNotFound()
        {
            // Arrange
            var _ = await _client.CreateSingleContactAsync(_scope);
            var invalidId = Guid.NewGuid();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/contacts/{invalidId}", _scope);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = Factories.Contacts.GenerateInValidCreateContactRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Post, "api/contacts", _scope, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AnswerAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = Factories.Contacts.GenerateInValidAnswerContactRequestDTO();
            var id = await _client.CreateSingleContactAsync(_scope);
            var accessToken = await _client.GetSuperAdminAccessTokenAsync(_scope);
            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, $"api/contacts/{id}/answer", _scope, request, accessToken: accessToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var getResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/contacts/{id}", _scope, accessToken: accessToken);
            var contact = await getResponse.Content.ReadFromJsonAsync<GetContactResponseDTO>();

            contact.Should().NotBeNull();
            contact.IsAnswered.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_WhenProvidingInValidRequest_ShouldReturnNotFound()
        {
            // Arrange
            const byte contactsCount = 1;
            const byte countAfterDelete = contactsCount; // if invalid request, count should be the same
            var ids = await _client.CreateMultipleContactsAsync(_scope, contactsCount);

            var id = ids.First();
            var invalidId = Guid.NewGuid();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Delete, $"api/contacts/{invalidId}", _scope);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var getAllResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/contacts", _scope);

            var contacts = await getAllResponse.Content.ReadFromJsonAsync<IEnumerable<GetContactResponseDTO>>();

            contacts.Should().NotBeEmpty();
            contacts.Count().Should().Be(countAfterDelete);
            contacts.Select(contact => contact.Id).Contains(id).Should().BeTrue();
        }
    }
}
