namespace InternIntelligence_Portfolio.API.Endpoints
{
    public static class Auth
    {
        public static IEndpointRouteBuilder RegisterAuthEndpoints(this IEndpointRouteBuilder routes)
        {
            var auth = routes.MapGroup("api/auth").AllowAnonymous();

            //auth.MapPost("login", async (IAuthService authService, [FromBody] LoginDTO loginDto) =>
            //{
            //    var tokenDto = await authService.LoginAsync(loginDto);

            //    return Results.Ok(tokenDto);
            //}).Validate<LoginDTO>();

            return routes;
        }
    }
}
