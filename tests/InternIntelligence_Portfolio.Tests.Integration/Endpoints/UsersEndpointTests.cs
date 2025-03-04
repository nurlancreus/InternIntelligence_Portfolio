using FluentAssertions;
using InternIntelligence_Portfolio.Application.DTOs.User;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using InternIntelligence_Portfolio.Tests.Integration.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace InternIntelligence_Portfolio.Tests.Integration.Endpoints
{

    [Collection("Sequential")]
    public class UsersEndpointTests : IClassFixture<TestingWebApplicationFactory>, IAsyncLifetime
    {
        private readonly TestingWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly AppDbContext _context;

        public UsersEndpointTests(TestingWebApplicationFactory factory)
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

        [Fact]
        public async Task GetMeAsync_WhenProvidingValidRequest_ShouldReturnUserData()
        {
            // Arrange
            var userId = await _scope.RegisterSuperAdminAsync();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/users/me", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var userResponseDTO = await response.Content.ReadFromJsonAsync<GetUserResponseDTO>();

            userResponseDTO.Should().NotBeNull();
            userResponseDTO.Id.Should().Be(userId);
        }

        [Fact]
        public async Task ChangePictureAsync_WhenProvidingValidRequest_ShouldReturnTrue()
        {
            // Arrange
            var request = Factories.Users.GenerateValidChangeProfilePictureRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, "api/users/change-picture", _scope, request, true);

            // Assert
            response.EnsureSuccessStatusCode();
            var isSuccess = await response.Content.ReadFromJsonAsync<bool>();
            isSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task ChangePictureAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = Factories.Users.GenerateInValidChangeProfilePictureRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, "api/users/change-picture", _scope, request, true);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
