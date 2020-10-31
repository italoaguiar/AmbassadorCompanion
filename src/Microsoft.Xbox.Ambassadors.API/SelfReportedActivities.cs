using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public class SelfReportedActivity
    {
        private const string REQUEST_URI = "https://ambassadors-production.azure-api.net/api/activities/selfreported";
        private const string POST_URI = "https://ambassadors-production.azure-api.net/api/activities/";

        public static async Task<SelfReportedActivity[]> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<SelfReportedActivity[]>(token, new Uri(REQUEST_URI));
        }

        public static async Task<HttpResponseMessage> PostAsync(AccessToken token, uint activityId )
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, new Uri(POST_URI + activityId)))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                using (HttpClient http = new HttpClient())
                {
                    HttpResponseMessage response = await http.SendAsync(msg);
                    return response;
                }
            }
            
        }

        [JsonPropertyName("activityId")]
        public uint ActivityId { get; set; }

        [JsonPropertyName("activityTypeDefinitionId")]
        public long ActivityTypeDefinitionId { get; set; }

        [JsonPropertyName("specializationId")]
        public long SpecializationId { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("displayDescription")]
        public string DisplayDescription { get; set; }

        [JsonPropertyName("internalDescription")]
        public string InternalDescription { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("experiencePoints")]
        public long ExperiencePoints { get; set; }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("effectiveDate")]
        public object EffectiveDate { get; set; }

        [JsonPropertyName("expirationDate")]
        public object ExpirationDate { get; set; }

        [JsonPropertyName("modifiedDate")]
        public DateTimeOffset ModifiedDate { get; set; }

        [JsonPropertyName("isDisplayed")]
        public bool IsDisplayed { get; set; }

        [JsonPropertyName("payload")]
        public object Payload { get; set; }
    }
}
