using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
