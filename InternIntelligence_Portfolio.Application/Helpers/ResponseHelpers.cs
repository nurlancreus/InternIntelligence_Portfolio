using Microsoft.AspNetCore.Identity;

namespace InternIntelligence_Portfolio.Application.Helpers
{
    public static class ResponseHelpers
    {
        public static string GetResultErrorsMessage(IdentityResult result)
        {
            var messages = result.Errors.Select(e => e.Description);

            return string.Join(", ", messages);
        }
    }
}
