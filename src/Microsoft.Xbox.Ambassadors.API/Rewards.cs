using Microsoft.Xbox.Ambassadors.API.Auth;
using Microsoft.Xbox.Ambassadors.API.DataModel.Rewards;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public static class Rewards
    {
        const string REQUEST_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/rewards/season/";

        const string REWARDED_SESSIONS_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/rewards/rewardedseasons";

        public static async Task<Reward[]> GetRewardsAsync(AccessToken token, int seasonId)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Reward[]>(token, new Uri(REQUEST_URI + seasonId)).ConfigureAwait(false);
        }

        public static async Task<int[]> GetRewardedSessionsAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<int[]>(token, new Uri(REWARDED_SESSIONS_URI)).ConfigureAwait(false);
        }

        
    }
}

namespace Microsoft.Xbox.Ambassadors.API.DataModel.Rewards
{
    public class Reward
    {
        [JsonPropertyName("ambassadorRewardId")]
        public long AmbassadorRewardId { get; set; }

        [JsonPropertyName("rewardType")]
        public long RewardType { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("displayDescription")]
        public string DisplayDescription { get; set; }

        [JsonPropertyName("earnedDate")]
        public DateTimeOffset EarnedDate { get; set; }

        [JsonPropertyName("claimedDate")]
        public object ClaimedDate { get; set; }

        [JsonPropertyName("imageUri")]
        public Uri ImageUri { get; set; }

        [JsonPropertyName("payload")]
        public string Payload { get; set; }

        [JsonPropertyName("mtsRequestId")]
        public object MtsRequestId { get; set; }

        [JsonPropertyName("mtsDeliveryId")]
        public object MtsDeliveryId { get; set; }

        [JsonPropertyName("msAssetId")]
        public object MsAssetId { get; set; }

        [JsonPropertyName("assetSourceCode")]
        public object AssetSourceCode { get; set; }
    }
}
