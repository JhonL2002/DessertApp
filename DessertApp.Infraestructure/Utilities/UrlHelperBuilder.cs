
namespace DessertApp.Infraestructure.Utilities
{
    public static class UrlHelperBuilder
    {
        public static string BuildConfirmationUrl(string baseUrl, string userId, string code, string returnUrl)
        {
            var uriBuilder = new UriBuilder(baseUrl);
            var query = new System.Collections.Specialized.NameValueCollection
            {
                { "userId", userId},
                { "code", code },
                { "returnUrl", returnUrl }
            };

            uriBuilder.Query = string.Join("&", query.AllKeys.Select(key => $"{key}={Uri.EscapeDataString(query[key]!)}"));
            return uriBuilder.ToString();
        }

        public static string BuildResetPasswordUrl(string baseUrl, string code, string returnUrl)
        {
            var uriBuilder = new UriBuilder(baseUrl);
            var query = new System.Collections.Specialized.NameValueCollection
            {
                { "code", code },
                { "returnUrl", returnUrl }
            };

            uriBuilder.Query = string.Join("&", query.AllKeys.Select(key => $"{key}={Uri.EscapeDataString(query[key]!)}"));
            return uriBuilder.ToString();
        }
    }
}
