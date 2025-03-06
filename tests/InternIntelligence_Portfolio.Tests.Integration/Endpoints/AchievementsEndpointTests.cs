using FluentAssertions;
using InternIntelligence_Portfolio.Application.DTOs.Achievement;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using InternIntelligence_Portfolio.Tests.Integration.Helpers;
using Microsoft.Extensions.DependencyInjection;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using System.Net;
using System.Net.Http.Json;

namespace InternIntelligence_Portfolio.Tests.Integration.Endpoints
{
    [Collection("Sequential")]
    public class AchievementsEndpointTests : IClassFixture<TestingWebApplicationFactory>, IAsyncLifetime
    {
        private readonly TestingWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly AppDbContext _context;

        public AchievementsEndpointTests(TestingWebApplicationFactory factory)
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
        public async Task GetAllAsync_WhenProvidingValidRequest_ShouldReturnAchievements()
        {
            // Arrange
            const byte achievementsCount = 5;
            var ids = await _client.CreateMultipleAchievementsAsync(_scope, achievementsCount);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/achievements", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var achievements = await response.Content.ReadFromJsonAsync<IEnumerable<GetAchievementResponseDTO>>();

            achievements.Should().NotBeEmpty();
            achievements.Count().Should().Be(achievementsCount);
            achievements.All(achievement => ids.Contains(achievement.Id)).Should().BeTrue();
        }

        [Fact]
        public async Task GetAsync_WhenProvidingValidRequest_ShouldReturnAchievement()
        {
            // Arrange
            var id = await _client.CreateSingleAchievementAsync(_scope);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/achievements/{id}", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var achievement = await response.Content.ReadFromJsonAsync<GetAchievementResponseDTO>();

            achievement.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAsync_WhenProvidingValidRequest_ShouldReturnId()
        {
            // Arrange
            var request = Factories.Achievements.GenerateValidCreateAchievementRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Post, "api/achievements", _scope, request);

            // Assert
            response.EnsureSuccessStatusCode();
            var achievementId = await response.Content.ReadFromJsonAsync<Guid>();
            achievementId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_WhenProvidingValidRequest_ShouldReturnId()
        {
            // Arrange
            var id = await _client.CreateSingleAchievementAsync(_scope);
            var request = Factories.Achievements.GenerateValidUpdateAchievementRequestDTO();
            var accessToken = await _client.GetSuperAdminAccessTokenAsync(_scope);

            var achievementResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/achievements/{id}", _scope, accessToken: accessToken);

            var achievement = await achievementResponse.Content.ReadFromJsonAsync<GetAchievementResponseDTO>();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, $"api/achievements/{id}", _scope, request, accessToken: accessToken);

            // Assert
            response.EnsureSuccessStatusCode();

            var updatedAchievementId = await response.Content.ReadFromJsonAsync<Guid>();
            updatedAchievementId.Should().NotBeEmpty();

            var updatedAchievementResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/achievements/{id}", _scope, accessToken: accessToken);

            var updatedAchievement = await updatedAchievementResponse.Content.ReadFromJsonAsync<GetAchievementResponseDTO>();
            updatedAchievement.Should().NotBeNull();

            achievement.Should().NotBeNull();
            updatedAchievement.Title.Should().NotBeEquivalentTo(achievement.Title);
        }

        [Fact]
        public async Task DeleteAsync_WhenProvidingValidRequest_ShouldReturnTrue()
        {
            // Arrange
            const byte achievementsCount = 5;
            const byte countAfterDelete = achievementsCount - 1;
            var ids = await _client.CreateMultipleAchievementsAsync(_scope, achievementsCount);

            var id = ids.First();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Delete, $"api/achievements/{id}", _scope);

            // Assert
            response.EnsureSuccessStatusCode();

            var getAllResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/achievements", _scope);

            var achievements = await getAllResponse.Content.ReadFromJsonAsync<IEnumerable<GetAchievementResponseDTO>>();

            achievements.Should().NotBeEmpty();
            achievements.Count().Should().Be(countAfterDelete);
            achievements.Select(achievement => achievement.Id).Contains(id).Should().BeFalse();
        }

        // Invalid tests
        [Fact]
        public async Task GetAllAsync_WhenProvidingExtraRequest_ShouldReturnWrongCount()
        {
            // Arrange
            const byte achievementsCount = 5;
            const byte achievementsCountIfExtraEntityExist = achievementsCount + 1;

            var id = await _client.CreateSingleAchievementAsync(_scope);
            var ids = await _client.CreateMultipleAchievementsAsync(_scope, achievementsCount);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/achievements", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var achievements = await response.Content.ReadFromJsonAsync<IEnumerable<GetAchievementResponseDTO>>();

            achievements.Should().NotBeEmpty();
            achievements.Count().Should().Be(achievementsCountIfExtraEntityExist);
            achievements.FirstOrDefault(achievement => !ids.Contains(achievement.Id))?.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetAsync_WhenProvidingInValidRequest_ShouldReturnNotFound()
        {
            // Arrange
            var _ = await _client.CreateSingleAchievementAsync(_scope);
            var invalidId = Guid.NewGuid();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/achievements/{invalidId}", _scope);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = Factories.Achievements.GenerateInValidCreateAchievementRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Post, "api/achievements", _scope, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var id = await _client.CreateSingleAchievementAsync(_scope);
            var request = Factories.Achievements.GenerateInValidUpdateAchievementRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, $"api/achievements/{id}", _scope, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAsync_WhenProvidingInValidRequest_ShouldReturnNotFound()
        {
            // Arrange
            const byte achievementsCount = 5;
            const byte countAfterDelete = achievementsCount; // if invalid request, count should be the same
            var ids = await _client.CreateMultipleAchievementsAsync(_scope, achievementsCount);

            var id = ids.First();
            var invalidId = Guid.NewGuid();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Delete, $"api/achievements/{invalidId}", _scope);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var getAllResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/achievements", _scope);

            var achievements = await getAllResponse.Content.ReadFromJsonAsync<IEnumerable<GetAchievementResponseDTO>>();

            achievements.Should().NotBeEmpty();
            achievements.Count().Should().Be(countAfterDelete);
            achievements.Select(achievement => achievement.Id).Contains(id).Should().BeTrue();
        }

        [Fact]
        public async Task GetAllAsync_WhenProvidingInValidAccessToken_ShouldReturnUnAuthorized()
        {
            // Arrange
            string invalidAccessToken = Factories.Auth.GenerateInValidAccessToken();
            const byte achievementsCount = 5;

            var _ = await _client.CreateMultipleAchievementsAsync(_scope, achievementsCount);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/achievements", _scope, accessToken: invalidAccessToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAsync_WhenProvidingInValidAccessToken_ShouldReturnUnAuthorized()
        {
            // Arrange
            var id = await _client.CreateSingleAchievementAsync(_scope);
            string invalidAccessToken = Factories.Auth.GenerateInValidAccessToken();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/achievements/{id}", _scope, accessToken: invalidAccessToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreateAsync_WhenProvidingInValidAccessToken_ShouldReturnUnAuthorized()
        {
            // Arrange
            string invalidAccessToken = Factories.Auth.GenerateInValidAccessToken();
            var request = Factories.Achievements.GenerateValidCreateAchievementRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Post, "api/achievements", _scope, request, accessToken: invalidAccessToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task UpdateAsync_WhenProvidingInValidAccessToken_ShouldReturnUnAuthorized()
        {
            // Arrange
            string invalidAccessToken = Factories.Auth.GenerateInValidAccessToken();

            var id = await _client.CreateSingleAchievementAsync(_scope);
            var request = Factories.Achievements.GenerateValidUpdateAchievementRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, $"api/achievements/{id}", _scope, request, accessToken: invalidAccessToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task DeleteAsync_WhenProvidingInValidAccessToken_ShouldReturnUnAuthorized()
        {
            // Arrange
            string invalidAccessToken = Factories.Auth.GenerateInValidAccessToken();

            const byte achievementsCount = 5;

            var ids = await _client.CreateMultipleAchievementsAsync(_scope, achievementsCount);

            var id = ids.First();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Delete, $"api/achievements/{id}", _scope, accessToken: invalidAccessToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
