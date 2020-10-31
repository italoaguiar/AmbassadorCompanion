using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Microsoft.Xbox.Ambassadors.API
{
    public partial class Videos
    {
        const string REQUEST_URI = "https://ambassadors.search.windows.net/indexes/youtubecloud-search-index/docs?api-version=2016-09-01" +
            "&search=*" +
            "&$filter=state%20eq%200%20and%20expirationDate%20ge%20{0}" +
            "&$orderby=lastActivityDate%20desc" +
            "&$top={1}" +
            "&$count=true" +
            "&$skip={2}";

        const string VOTE_URI = "https://ambassadors-production.azure-api.net/api/videos/votes/";
        const string VIDEO_URI = "https://ambassadors-production.azure-api.net/api/videos";


        [JsonPropertyName("@odata.context")]
        public Uri OdataContext { get; set; }

        [JsonPropertyName("@odata.count")]
        public long OdataCount { get; set; }

        [JsonPropertyName("value")]
        public IList<Value> Value { get; set; }

        public static async Task<Videos> GetAsync(AccessToken token,uint top = 9, uint skip = 0)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            string date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string req = string.Format(CultureInfo.InvariantCulture, REQUEST_URI, date, top, skip);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("api-key", "37A715329C9FCA3ED6297F9B13E3478D");


            return await HttpUtil.GetAsync<Videos>(token, new Uri(req), dic);
        }

        public static async Task<bool> PostVoteAsync(AccessToken token, string videoId, string reason = "")
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            string body = $"{{\"videoId\":\"{videoId}\",\"reason\":\"{reason}\"}}";

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, new Uri(VOTE_URI)))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                msg.Content = new StringContent(body);
                msg.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                using (HttpClient http = new HttpClient())
                {
                    HttpResponseMessage response = await http.SendAsync(msg);
                    return response.IsSuccessStatusCode;
                }
            }
        }

        public static async Task<bool> PostVideoAsync(AccessToken token, string videoId)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            string body = $"{{\"videoId\":\"{videoId}\",\"reason\":\"\"}}";

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, new Uri(VIDEO_URI)))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                msg.Content = new StringContent(body);
                msg.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                using (HttpClient http = new HttpClient())
                {
                    HttpResponseMessage response = await http.SendAsync(msg);
                    return response.IsSuccessStatusCode;
                }
            }
        }
    }

    public partial class Value
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("videoId")]
        public string VideoId { get; set; }

        [JsonPropertyName("channelId")]
        public string ChannelId { get; set; }

        [JsonPropertyName("submittedOn")]
        public DateTimeOffset SubmittedOn { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("viewCount")]
        public long ViewCount { get; set; }

        [JsonPropertyName("videoLink")]
        public Uri VideoLink { get; set; }

        [JsonPropertyName("state")]
        public long State { get; set; }

        [JsonPropertyName("expirationDate")]
        public DateTimeOffset ExpirationDate { get; set; }

        [JsonPropertyName("lastActivityDate")]
        public DateTimeOffset LastActivityDate { get; set; }

        [JsonPropertyName("upvoteCount")]
        public long UpvoteCount { get; set; }

        [JsonPropertyName("downvoteCount")]
        public long DownvoteCount { get; set; }

        [JsonPropertyName("lastVoteTimestamp")]
        public long LastVoteTimestamp { get; set; }
    }
}
