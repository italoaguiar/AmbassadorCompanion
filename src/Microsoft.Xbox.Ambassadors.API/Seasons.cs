using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public partial class Season
    {
        const string REQUEST_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/seasons";

        public static async Task<Season[]> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Season[]>(token, new Uri(REQUEST_URI));
        }

        public static async Task<Season> GetCurrentAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return (await HttpUtil.GetAsync<Season[]>(token, new Uri($"{REQUEST_URI}?current=true")))[0];
        }

        [JsonPropertyName("seasonId")]
        public long SeasonId { get; set; }

        [JsonPropertyName("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("totalDays")]
        public long TotalDays { get; set; }

        [JsonPropertyName("daysLeft")]
        public long DaysLeft { get; set; }
    }
}
