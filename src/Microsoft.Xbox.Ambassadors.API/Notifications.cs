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
    public static class Notifications
    {
        const string REQUEST_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/notifications";

        public static async Task<Notification[]> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Notification[]>(token, new Uri(REQUEST_URI));
        }

        public static async Task<bool> RemoveAsync(AccessToken token, string id)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, new Uri($"{REQUEST_URI}/{id}")))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                using (HttpClient http = new HttpClient())
                {
                    HttpResponseMessage response = await http.SendAsync(msg);
                    return response.IsSuccessStatusCode;
                }
            }
        }
    }

    public class Notification
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("sentDateTime")]
        public SentDateTime SentDateTime { get; set; }

        [JsonPropertyName("isCompleted")]
        public bool IsCompleted { get; set; }

        [JsonPropertyName("isDismissible")]
        public bool IsDismissible { get; set; }

        [JsonPropertyName("isDismissed")]
        public bool IsDismissed { get; set; }

        [JsonPropertyName("isNew")]
        public bool IsNew { get; set; }

        [JsonPropertyName("payload")]
        public object Payload { get; set; }
    }

    public partial class SentDateTime
    {
        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("epoch")]
        public long Epoch { get; set; }
    }
}
