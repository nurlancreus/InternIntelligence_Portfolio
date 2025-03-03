using InternIntelligence_Portfolio.Application.DTOs.Token;
using InternIntelligence_Portfolio.Application.Helpers;
using InternIntelligence_Portfolio.Domain.Entities.Identity;
using InternIntelligence_Portfolio.Tests.Common.Constants;
using InternIntelligence_Portfolio.Tests.Common.Factories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InternIntelligence_Portfolio.Tests.Integration
{
    public static class Extensions
    {
        public async static Task<Guid> CreateSingleSkillAsync(this HttpClient client, IServiceScope scope)
        {
            var request = Factories.Skills.GenerateValidCreateSkillsRequestDTO();

            var response = await client.SendRequestWithAccessToken(HttpMethod.Post, "api/skills", scope, request);

            response.EnsureSuccessStatusCode();

            var skillId = await response.Content.ReadFromJsonAsync<Guid>();

            return skillId;

        }

        public async static Task<IEnumerable<Guid>> CreateMultipleSkillsAsync(this HttpClient client, IServiceScope scope, byte count = 3)
        {
            var requestDtos = Factories.Skills.GenerateMultipleValidCreateSkillRequestDTOs(count);

            List<Guid> skillIds = [];

            foreach (var requestDTO in requestDtos)
            {
                var response = await client.SendRequestWithAccessToken(HttpMethod.Post, "api/skills", scope, requestDTO);

                response.EnsureSuccessStatusCode();

                var skillId = await response.Content.ReadFromJsonAsync<Guid>();

                skillIds.Add(skillId);
            }

            return skillIds;
        }

        public async static Task<Guid> RegisterSuperAdminAsync(this IServiceScope scope)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var superAdminUser = await userManager.FindByNameAsync(Constants.Auth.UserName_Valid);

            if (superAdminUser is null)
            {
                superAdminUser = Factories.Auth.GenerateValidSuperAdmin();

                var result = await userManager.CreateAsync(superAdminUser, Constants.Auth.Pw_Valid);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Could not register super admin user for testing purposes: {ResponseHelpers.GetResultErrorsMessage(result)}");
                }
            }

            var superAdminRole = await roleManager.FindByNameAsync(Constants.Auth.SuperAdmin_Role);

            if (superAdminRole is null)
            {
                superAdminRole = new IdentityRole<Guid>(Constants.Auth.SuperAdmin_Role);

                var result = await roleManager.CreateAsync(superAdminRole);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Could not register super admin role for testing purposes: {ResponseHelpers.GetResultErrorsMessage(result)}");
                }
            }

            if (!await userManager.IsInRoleAsync(superAdminUser, Constants.Auth.SuperAdmin_Role))
            {
                var result = await userManager.AddToRoleAsync(superAdminUser, Constants.Auth.SuperAdmin_Role);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Could not add role 'SuperAdmin' for superadmin user for testing purposes: {ResponseHelpers.GetResultErrorsMessage(result)}");
                }
            }

            return superAdminUser.Id;
        }

        public async static Task<string> GetSuperAdminAccessTokenAsync(this HttpClient client, IServiceScope scope)
        {
            await scope.RegisterSuperAdminAsync();

            var request = Factories.Auth.GenerateValidLoginRequestDTO();

            var loginResponse = await client.PostAsJsonAsync("api/auth/login", request);
            loginResponse.EnsureSuccessStatusCode();

            var tokenResponse = await loginResponse.Content.ReadFromJsonAsync<TokenDTO>();
            return tokenResponse!.AccessToken;
        }

        public async static Task<HttpResponseMessage> SendRequestWithAccessToken(this HttpClient client, HttpMethod httpMethod, string requestUrl, IServiceScope scope, object? requestBody = null, bool isFromForm = false)
        {
            var accessToken = await client.GetSuperAdminAccessTokenAsync(scope);
            var request = new HttpRequestMessage(httpMethod, requestUrl);

            if (requestBody != null)
            {
                if (isFromForm)
                {
                    request.Content = ConvertToMultipartFormData(requestBody);
                }
                else
                {
                    var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await client.SendAsync(request);
        }

        private static MultipartFormDataContent ConvertToMultipartFormData(object requestBody)
        {
            var formData = new MultipartFormDataContent();
            var type = requestBody.GetType();

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(requestBody);

                if (value == null)
                    continue;

                if (value is IFormFile formFile)
                {
                    var fileContent = new StreamContent(formFile.OpenReadStream());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                    formData.Add(fileContent, property.Name, formFile.FileName);
                }
                else
                {
                    formData.Add(new StringContent(value.ToString()!), property.Name);
                }
            }

            return formData;
        }
    }
}
