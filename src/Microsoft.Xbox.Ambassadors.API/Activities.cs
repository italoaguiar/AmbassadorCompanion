using Microsoft.Xbox.Ambassadors.API.Auth;
using Microsoft.Xbox.Ambassadors.API.DataModel.Activities;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Microsoft.Xbox.Ambassadors.API
{
    public static class Activities
    {
        const string REQUEST_URI = "https://ambassadors-production.azure-api.net/api/activities?type={0}&page={1}";

        public async static Task<DailyActivities[]> GetAsync(AccessToken token, ActivityType aType, int page = 1)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            string req = string.Format(CultureInfo.InvariantCulture, REQUEST_URI, aType, page);
            return await HttpUtil.GetAsync<DailyActivities[]>(token, new Uri(req));
        }
    }

    public enum ActivityType
    {
        activities,
        reactions
    }
}
namespace Microsoft.Xbox.Ambassadors.API.DataModel.Activities
{
    public partial class DailyActivities
    {
        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("activityList")]
        public IList<ActivityList> ActivityList { get; set; }
    }

    public partial class ActivityList
    {
        [JsonPropertyName("activityId")]
        public long ActivityId { get; set; }

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
        public string Payload { get; set; }
    }
}
