using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public static class ErrorCode
    {
        const string REQUEST_URI = "http://xboxambassadors.azurewebsites.net/api/help?q=";

        public static async Task<ErrorDetail[]> GetAsync(string query)
        {
            return await HttpUtil.GetAsync<ErrorDetail[]>(new Uri($"{REQUEST_URI}{query}"));
        }
    }

    public class ErrorDetail
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("raw")]
        public string Raw { get; set; }

        [JsonPropertyName("html")]
        public string Html { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("lang")]
        public string Lang { get; set; }
    }
}
