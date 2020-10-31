using Microsoft.Xbox.Ambassadors.API.Auth;
using Microsoft.Xbox.Ambassadors.API.DataModel.Specializations;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Microsoft.Xbox.Ambassadors.API
{
    public static class Specializations
    {
        const string REQUEST_URI = "https://ambassadors-production.azure-api.net/api/profiles/specializations";

        public async static Task<Specialization[]> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Specialization[]>(token, new Uri(REQUEST_URI));
        }

        public async static Task<bool> PutAsync(AccessToken token, Specialization[] data)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Put, new Uri(REQUEST_URI)))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                msg.Content = new StringContent(JsonSerializer.Serialize(data));
                msg.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                using (HttpClient http = new HttpClient())
                {
                    HttpResponseMessage response = await http.SendAsync(msg);
                    return response.IsSuccessStatusCode;
                }
            }
        }
    }
}

namespace Microsoft.Xbox.Ambassadors.API.DataModel.Specializations
{
    public class Specialization
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("preferences")]
        public IList<Preference> Preferences { get; set; }
    }

    public class Preference
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isSelected")]
        public bool IsSelected { get; set; }
    }
}
