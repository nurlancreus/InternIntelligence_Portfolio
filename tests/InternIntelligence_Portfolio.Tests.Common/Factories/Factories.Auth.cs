using InternIntelligence_Portfolio.Application.DTOs.Auth;
using InternIntelligence_Portfolio.Domain.Entities.Identity;

namespace InternIntelligence_Portfolio.Tests.Common.Factories
{
    public static partial class Factories
    {
        public static class Auth
        {
            public static string GenerateInValidAccessToken() => Constants.Constants.Auth.InValidAccessToken;

            public static ApplicationUser GenerateValidSuperAdmin()
            {
                return ApplicationUser.Create(
                    Constants.Constants.Auth.FirstName_Valid,
                    Constants.Constants.Auth.LastName_Valid,
                    Constants.Constants.Auth.UserName_Valid,
                    Constants.Constants.Auth.Email_Valid);
            }

            public static LoginRequestDTO GenerateValidLoginRequestDTO()
            {
                return new LoginRequestDTO
                {
                    UserName = Constants.Constants.Auth.UserName_Valid,
                    Password = Constants.Constants.Auth.Pw_Valid,
                };
            }

            public static LoginRequestDTO GenerateInValidLoginRequestDTO()
            {
                return new LoginRequestDTO
                {
                    UserName = Constants.Constants.Auth.UserName_InValid,
                    Password = Constants.Constants.Auth.Pw_InValid,
                };
            }
        }
    }
}
