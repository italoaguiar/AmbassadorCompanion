using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Microsoft.Xbox.Ambassadors.API
{
    public partial class Leaderboard
    {
        const string REQUEST_URI = "https://ambassadors-production.azure-api.net/api/leaderboard/{0}?page={1}&pagesize={2}";

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("totalCount")]
        public long TotalCount { get; set; }

        [JsonPropertyName("currentPage")]
        public long CurrentPage { get; set; }

        [JsonPropertyName("memberExists")]
        public bool? MemberExists { get; set; }

        public static async Task<Leaderboard> GetAsync(AccessToken token, uint season, uint pageSize = 8, uint? page = null)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            string url =  string.Format(CultureInfo.InvariantCulture, REQUEST_URI, season, page, pageSize);

            return await HttpUtil.GetAsync<Leaderboard>(token, new Uri(url));
        }

        public static async Task<Item> GetAsync(AccessToken token, uint seasonId, string gamertag)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            string url = string.Format(CultureInfo.InvariantCulture, "https://ambassadors.westus.cloudapp.azure.com:8637/api/leaderboard/{0}/{1}", seasonId, gamertag);

            return await HttpUtil.GetAsync<Item>(token, new Uri(url));
        }

        //unauthenticated endpoint has been disabled by microsoft
        //public static async Task<Leaderboard> GetAsync(uint season, uint page, uint pageSize = 100)
        //{
        //    string url = string.Format(REQUEST_URI, season, page, pageSize);

        //    return await HttpUtil.GetAsync<Leaderboard>(new Uri(url));
        //}


    }

    public partial class Item
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        [JsonPropertyName("experiencePoints")]
        public long ExperiencePoints { get; set; }

        [JsonPropertyName("totalActivitiesCompleted")]
        public long TotalActivitiesCompleted { get; set; }

        [JsonPropertyName("lastRewardEarnedTrophyURL")]
        public Uri LastRewardEarnedTrophyUrl { get; set; }

        [JsonPropertyName("joinedDateTime")]
        public DateTimeOffset JoinedDateTime { get; set; }

        [JsonPropertyName("legacyJoinedDateTime")]
        public object LegacyJoinedDateTime { get; set; }

        [JsonPropertyName("firstName")]
        public object FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public object LastName { get; set; }
    }

}
