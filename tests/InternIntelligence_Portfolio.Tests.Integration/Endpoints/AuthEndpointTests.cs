using FluentAssertions;
using InternIntelligence_Portfolio.Application.DTOs.Token;
using InternIntelligence_Portfolio.Infrastructure.Persistence.Context;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using InternIntelligence_Portfolio.Tests.Integration.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace InternIntelligence_Portfolio.Tests.Integration.Endpoints
{
    [Collection("Sequential")]
    public class AuthEndpointTests : IClassFixture<TestingWebApplicationFactory>, IAsyncLifetime
    {
        private readonly TestingWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly AppDbContext _context;

        public AuthEndpointTests(TestingWebApplicationFactory factory)
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
        public async Task LoginAsync_WhenProvidingValidRequest_ShouldReturnToken()
        {
            // Arrange
            await _scope.RegisterSuperAdminAsync();
            var request = Factories.Auth.GenerateValidLoginRequestDTO();

            // Act
            var response = await _client.PostAsJsonAsync("api/auth/login", request);

            // Assert
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadFromJsonAsync<TokenDTO>();

            token.Should().NotBeNull();
            token.AccessToken.Should().NotBeNull();
            token.RefreshToken.Should().NotBeNull();
            token.AccessTokenEndDate.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task LoginAsync_WhenProvidingInValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            await _scope.RegisterSuperAdminAsync();
            var request = Factories.Auth.GenerateInValidLoginRequestDTO();

            // Act
            var response = await _client.PostAsJsonAsync("api/auth/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
