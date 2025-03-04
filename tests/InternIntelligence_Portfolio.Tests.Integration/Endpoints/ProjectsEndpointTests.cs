using FluentAssertions;
using InternIntelligence_Portfolio.Application.DTOs.Project;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using InternIntelligence_Portfolio.Tests.Integration.Helpers;
using Microsoft.Extensions.DependencyInjection;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using System.Net;
using System.Net.Http.Json;

namespace InternIntelligence_Portfolio.Tests.Integration.Endpoints
{
    [Collection("Sequential")]
    public class ProjectsEndpointTests : IClassFixture<TestingWebApplicationFactory>, IAsyncLifetime
    {
        private readonly TestingWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly AppDbContext _context;

        public ProjectsEndpointTests(TestingWebApplicationFactory factory)
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
        public async Task GetAllAsync_WhenProvidingValidRequest_ShouldReturnProjects()
        {
            // Arrange
            const byte projectsCount = 5;
            var ids = await _client.CreateMultipleProjectsAsync(_scope, projectsCount);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/projects", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var projects = await response.Content.ReadFromJsonAsync<IEnumerable<GetProjectResponseDTO>>();

            projects.Should().NotBeEmpty();
            projects.Count().Should().Be(projectsCount);
            projects.All(project => ids.Contains(project.Id)).Should().BeTrue();
        }

        [Fact]
        public async Task GetAsync_WhenProvidingValidRequest_ShouldReturnProject()
        {
            // Arrange
            var id = await _client.CreateSingleProjectAsync(_scope);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/projects/{id}", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var project = await response.Content.ReadFromJsonAsync<GetProjectResponseDTO>();

            project.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateAsync_WhenProvidingValidRequest_ShouldReturnId()
        {
            // Arrange
            var request = Factories.Projects.GenerateValidCreateProjectRequestDTO(true);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Post, "api/projects", _scope, request, isFromForm: true);

            // Assert
            response.EnsureSuccessStatusCode();
            var projectId = await response.Content.ReadFromJsonAsync<Guid>();
            projectId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_WhenProvidingValidRequest_ShouldReturnId()
        {
            // Arrange
            var id = await _client.CreateSingleProjectAsync(_scope);
            var request = Factories.Projects.GenerateValidUpdatedProjectRequestDTO();
            var accessToken = await _client.GetSuperAdminAccessTokenAsync(_scope);

            var projectResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/projects/{id}", _scope, accessToken: accessToken);

            var project = await projectResponse.Content.ReadFromJsonAsync<GetProjectResponseDTO>();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, $"api/projects/{id}", _scope, request, accessToken: accessToken, isFromForm: true);

            // Assert
            response.EnsureSuccessStatusCode();

            var updatedProjectId = await response.Content.ReadFromJsonAsync<Guid>();
            updatedProjectId.Should().NotBeEmpty();

            var updatedProjectResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/projects/{id}", _scope, accessToken: accessToken);

            var updatedProject = await updatedProjectResponse.Content.ReadFromJsonAsync<GetProjectResponseDTO>();
            updatedProject.Should().NotBeNull();

            project.Should().NotBeNull();
            updatedProject.Name.Should().NotBeEquivalentTo(project.Name);
        }

        [Fact]
        public async Task DeleteAsync_WhenProvidingValidRequest_ShouldReturnTrue()
        {
            // Arrange
            const byte projectsCount = 5;
            const byte countAfterDelete = projectsCount - 1;
            var ids = await _client.CreateMultipleProjectsAsync(_scope, projectsCount);

            var id = ids.First();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Delete, $"api/projects/{id}", _scope);

            // Assert
            response.EnsureSuccessStatusCode();

            var getAllResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/projects", _scope);

            var projects = await getAllResponse.Content.ReadFromJsonAsync<IEnumerable<GetProjectResponseDTO>>();

            projects.Should().NotBeEmpty();
            projects.Count().Should().Be(countAfterDelete);
            projects.Select(project => project.Id).Contains(id).Should().BeFalse();
        }

        // Invalid tests
        [Fact]
        public async Task GetAllAsync_WhenProvidingExtraRequest_ShouldReturnWrongCount()
        {
            // Arrange
            const byte projectsCount = 5;
            const byte projectssCountIfExtraEntityExist = projectsCount + 1;

            var id = await _client.CreateSingleProjectAsync(_scope);
            var ids = await _client.CreateMultipleProjectsAsync(_scope, projectsCount);

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/projects", _scope);

            // Assert
            response.EnsureSuccessStatusCode();
            var projects = await response.Content.ReadFromJsonAsync<IEnumerable<GetProjectResponseDTO>>();

            projects.Should().NotBeEmpty();
            projects.Count().Should().Be(projectssCountIfExtraEntityExist);
            projects.FirstOrDefault(project => !ids.Contains(project.Id))?.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetAsync_WhenProvidingInValidRequest_ShouldReturnNotFound()
        {
            // Arrange
            var _ = await _client.CreateSingleProjectAsync(_scope);
            var invalidId = Guid.NewGuid();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Get, $"api/projects/{invalidId}", _scope);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = Factories.Projects.GenerateInValidCreateProjectRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Post, "api/projects", _scope, request, isFromForm: true);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var id = await _client.CreateSingleProjectAsync(_scope);
            var request = Factories.Projects.GenerateInValidCreateProjectRequestDTO();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Patch, $"api/projects/{id}", _scope, request, isFromForm: true);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAsync_WhenProvidingInValidRequest_ShouldReturnNotFound()
        {
            // Arrange
            const byte projectsCount = 5;
            const byte countAfterDelete = projectsCount; // if invalid request, count should be the same
            var ids = await _client.CreateMultipleProjectsAsync(_scope, projectsCount);

            var id = ids.First();
            var invalidId = Guid.NewGuid();

            // Act
            var response = await _client.SendRequestWithAccessToken(HttpMethod.Delete, $"api/projects/{invalidId}", _scope);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var getAllResponse = await _client.SendRequestWithAccessToken(HttpMethod.Get, "api/projects", _scope);

            var projects = await getAllResponse.Content.ReadFromJsonAsync<IEnumerable<GetProjectResponseDTO>>();

            projects.Should().NotBeEmpty();
            projects.Count().Should().Be(countAfterDelete);
            projects.Select(project => project.Id).Contains(id).Should().BeTrue();
        }
    }
}
