using FluentAssertions;
using InternIntelligence_Portfolio.Application.DTOs.Skill;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace InternIntelligence_Portfolio.Tests.Integration.Endpoints
{
    [Collection("Sequential")]
    public class SkillsEndpointTests : IClassFixture<TestingWebApplicationFactory>, IAsyncLifetime
    {
        private readonly TestingWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly AppDbContext _context;

        public SkillsEndpointTests(TestingWebApplicationFactory factory)
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
        public async Task GetAllAsync_WhenProvidingValidRequest_ShouldReturnSkills()
        {
            // Arrange
            const byte skillsCount = 5;
            var ids = await _client.CreateMultipleSkillsAsync(_scope, skillsCount);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/skills", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var skills = await response.Content.ReadFromJsonAsync<IEnumerable<GetSkillResponseDTO>>();

            skills.Should().NotBeEmpty();
            skills.Count().Should().Be(skillsCount);
            skills.All(skill => ids.Contains(skill.Id)).Should().BeTrue();
        }

        [Fact]
        public async Task GetAsync_WhenProvidingValidRequest_ShouldReturnSkill()
        {
            // Arrange
            var id = await _client.CreateSingleSkillAsync(_scope);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/skills/{id}", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var skill = await response.Content.ReadFromJsonAsync<GetSkillResponseDTO>();

            skill.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAsync_WhenProvidingValidRequest_ShouldReturnId()
        {
            // Arrange
            var request = Factories.Skills.GenerateValidCreateSkillsRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Post, "api/skills", _scope, request);

            // Assert
            response.EnsureSuccessStatusCode();
            var skillId = await response.Content.ReadFromJsonAsync<Guid>();
            skillId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_WhenProvidingValidRequest_ShouldReturnId()
        {
            // Arrange
            var id = await _client.CreateSingleSkillAsync(_scope);
            var request = Factories.Skills.GenerateValidUpdateSkillsRequestDTO();

            var skillResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/skills/{id}", _scope);

            var skill = await skillResponse.Content.ReadFromJsonAsync<GetSkillResponseDTO>();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, $"api/skills/{id}", _scope, request);

            // Assert
            response.EnsureSuccessStatusCode();

            var updatedSkillId = await response.Content.ReadFromJsonAsync<Guid>();
            updatedSkillId.Should().NotBeEmpty();

            var updatedSkillResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/skills/{id}", _scope);

            var updatedSkill = await updatedSkillResponse.Content.ReadFromJsonAsync<GetSkillResponseDTO>();
            updatedSkill.Should().NotBeNull();

            skill.Should().NotBeNull();
            updatedSkill.Name.Should().NotBeEquivalentTo(skill.Name);
        }

        // Invalid tests
        [Fact]
        public async Task GetAllAsync_WhenProvidingExtraRequest_ShouldReturnWrongCount()
        {
            // Arrange
            const byte skillsCount = 5;
            var id = await _client.CreateSingleSkillAsync(_scope);
            var ids = await _client.CreateMultipleSkillsAsync(_scope, skillsCount);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/skills", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var skills = await response.Content.ReadFromJsonAsync<IEnumerable<GetSkillResponseDTO>>();

            skills.Should().NotBeEmpty();
            skills.Count().Should().Be(skillsCount + 1);
            skills.FirstOrDefault(skill => !ids.Contains(skill.Id))?.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetAsync_WhenProvidingInValidRequest_ShouldReturnNotFound()
        {
            // Arrange
            var _ = await _client.CreateSingleSkillAsync(_scope);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/skills/{Guid.NewGuid()}", _scope);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = Factories.Skills.GenerateInValidCreateSkillsRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Post, "api/skills", _scope, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var id = await _client.CreateSingleSkillAsync(_scope);
            var request = Factories.Skills.GenerateInValidUpdateSkillsRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, $"api/skills/{id}", _scope, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
