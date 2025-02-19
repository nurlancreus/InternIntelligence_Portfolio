namespace InternIntelligence_Portfolio.Domain.Abstractions
{
    public enum ErrorType
    {
        None,
        Unauthorized,
        Login,
        BadRequest,
        Forbidden,
        NotFound,
        Conflict,
        Validation,
        Token,
        Unexpected,
        Unhandled
    }
}
