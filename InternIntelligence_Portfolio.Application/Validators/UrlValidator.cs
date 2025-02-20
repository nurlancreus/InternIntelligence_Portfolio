namespace InternIntelligence_Portfolio.Application.Validators
{
    public static class UrlValidator
    {
        public static bool IsUrlValid(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)) return true;

            return false;
        }
    }
}
