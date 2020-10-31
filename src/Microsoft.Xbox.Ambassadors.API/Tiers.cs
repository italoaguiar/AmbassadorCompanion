using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public class Tiers
    {
        const string REQUEST_URI = "https://ambassadors-production.azure-api.net/api/tiers";

        public static async Task<Tiers[]> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Tiers[]>(token, new Uri(REQUEST_URI));
        }

        [JsonPropertyName("missionDefinitionId")]
        public long MissionDefinitionId { get; set; }

        [JsonPropertyName("sort")]
        public long Sort { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("maxXP")]
        public long? MaxXp { get; set; }

        [JsonPropertyName("minXP")]
        public long MinXp { get; set; }
        
    }
}
