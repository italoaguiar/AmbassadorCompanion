using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.Xbox.Ambassadors.API.DataModel.Missions;

namespace Microsoft.Xbox.Ambassadors.API
{
    public partial class Missions
    {
        const string REQUEST_URI = "https://ambassadors-production.azure-api.net/api/missions?hasCompleted=";


        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("seasonId")]
        public long? SeasonId { get; set; }

        [JsonPropertyName("categories")]
        public IList<Category> Categories { get; set; }

        public IEnumerable<Mission> AllMissions
        {
            get
            {
                return Categories.SelectMany(x => x.Missions);
            }
        }

        public IEnumerable<Mission> TopMissions
        {
            get
            {
                return Categories.SelectMany(x => x.Missions).Take(8);
            }
        }


        public static async Task<Missions> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Missions>(token, new Uri(REQUEST_URI + "false"));
        }
        public static async Task<CompletedMission[]> GetCompletedAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<CompletedMission[]>(token, new Uri(REQUEST_URI + "true"));
        }

    }
}

namespace Microsoft.Xbox.Ambassadors.API.DataModel.Missions
{
    public partial class Category
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sortOrder")]
        public long SortOrder { get; set; }

        [JsonPropertyName("iconUrl")]
        public object IconUrl { get; set; }

        [JsonPropertyName("missions")]
        public IList<Mission> Missions { get; set; }
    }

    public partial class Mission
    {
        [JsonPropertyName("id")]
        public object Id { get; set; }

        [JsonPropertyName("missionDefinitionId")]
        public long MissionDefinitionId { get; set; }

        [JsonPropertyName("specializationId")]
        public long SpecializationId { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("displayDescription")]
        public string DisplayDescription { get; set; }

        [JsonPropertyName("tagline")]
        public string Tagline { get; set; }

        [JsonPropertyName("threshold")]
        public long Threshold { get; set; }

        [JsonPropertyName("aggregateTarget")]
        public long AggregateTarget { get; set; }

        [JsonPropertyName("parentId")]
        public object ParentId { get; set; }

        [JsonPropertyName("missionTypeDefinitionId")]
        public long MissionTypeDefinitionId { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("seasonId")]
        public long? SeasonId { get; set; }

        [JsonPropertyName("effectiveDate")]
        public DateTimeOffset EffectiveDate { get; set; }

        [JsonPropertyName("expirationDate")]
        public DateTimeOffset ExpirationDate { get; set; }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonPropertyName("missionCategoryId")]
        public long MissionCategoryId { get; set; }

        [JsonPropertyName("payload")]
        public object Payload { get; set; }

        [JsonPropertyName("missionActivities")]
        public IList<MissionActivity> MissionActivities { get; set; }

        [JsonPropertyName("missionRewards")]
        public IList<MissionReward> MissionRewards { get; set; }

        [JsonPropertyName("specialization")]
        public Specialization Specialization { get; set; }

        [JsonPropertyName("season")]
        public Season Season { get; set; }

        [JsonPropertyName("iconUri")]
        public Uri IconUri { get; set; }

        [JsonPropertyName("actorId")]
        public object ActorId { get; set; }

        [JsonPropertyName("isQuizMission")]
        public object IsQuizMission { get; set; }

        [JsonPropertyName("timeLeft")]
        public TimeLeft TimeLeft { get; set; }

        [JsonPropertyName("hasAccepted")]
        public bool HasAccepted { get; set; }

        [JsonPropertyName("hasCompleted")]
        public bool HasCompleted { get; set; }

        [JsonPropertyName("progress")]
        public long Progress { get; set; }

        [JsonIgnore]
        public long ProgressPercent
        {
            get
            {
                return Progress * 100 / Threshold;

            }
        }

        [JsonPropertyName("xpRequired")]
        public long XpRequired { get; set; }

        [JsonPropertyName("featured")]
        public bool Featured { get; set; }

        [JsonPropertyName("rewardSum")]
        public long RewardSum { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("acceptedDate")]
        public DateTimeOffset? AcceptedDate { get; set; }

        [JsonPropertyName("processedActivitiesCount")]
        public long? ProcessedActivitiesCount { get; set; }

        [JsonPropertyName("xPEarned")]
        public long? XPEarned { get; set; }

        [JsonPropertyName("lastActivityCompletedDate")]
        public DateTimeOffset? LastActivityCompletedDate { get; set; }

        [JsonPropertyName("wasAbandoned")]
        public bool? WasAbandoned { get; set; }

        [JsonPropertyName("missions")]
        public IList<Mission> Missions { get; set; }
    }

    public partial class MissionActivity
    {
        [JsonPropertyName("missionDefinitionId")]
        public long MissionDefinitionId { get; set; }

        [JsonPropertyName("activityId")]
        public long ActivityId { get; set; }

        [JsonPropertyName("activity")]
        public Activity Activity { get; set; }
    }

    public partial class Activity
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
        public object Url { get; set; }

        [JsonPropertyName("experiencePoints")]
        public long ExperiencePoints { get; set; }

        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("effectiveDate")]
        public DateTimeOffset? EffectiveDate { get; set; }

        [JsonPropertyName("expirationDate")]
        public DateTimeOffset? ExpirationDate { get; set; }

        [JsonPropertyName("modifiedDate")]
        public DateTimeOffset ModifiedDate { get; set; }

        [JsonPropertyName("isDisplayed")]
        public bool IsDisplayed { get; set; }

        [JsonPropertyName("payload")]
        public object Payload { get; set; }
    }

    public partial class MissionReward
    {
        [JsonPropertyName("missionDefinitionId")]
        public long MissionDefinitionId { get; set; }

        [JsonPropertyName("rewardDefinitionId")]
        public long RewardDefinitionId { get; set; }

        [JsonPropertyName("rewardDefinition")]
        public RewardDefinition RewardDefinition { get; set; }
    }


    public partial class Specialization
    {
        [JsonPropertyName("specializationId")]
        public long SpecializationId { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("displayDescription")]
        public string DisplayDescription { get; set; }

        [JsonPropertyName("iconUri")]
        public Uri IconUri { get; set; }

        [JsonPropertyName("parentId")]
        public long ParentId { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("expirationDate")]
        public DateTimeOffset ExpirationDate { get; set; }

        [JsonPropertyName("isPublished")]
        public bool IsPublished { get; set; }

        [JsonPropertyName("enrollmentType")]
        public long EnrollmentType { get; set; }

        [JsonPropertyName("externalResource")]
        public object ExternalResource { get; set; }
    }

    public partial class TimeLeft
    {
        [JsonPropertyName("minutes")]
        public long Minutes { get; set; }

        [JsonPropertyName("hours")]
        public long Hours { get; set; }

        [JsonPropertyName("days")]
        public long Days { get; set; }
    }

    public class CompletedMission
    {
        [JsonPropertyName("ambassadorProfileId")]
        public long AmbassadorProfileId { get; set; }

        [JsonPropertyName("missionDefinitionId")]
        public long MissionDefinitionId { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("closedDate")]
        public DateTimeOffset ClosedDate { get; set; }

        [JsonPropertyName("processedActivitiesCount")]
        public long ProcessedActivitiesCount { get; set; }

        [JsonPropertyName("xPEarned")]
        public long XPEarned { get; set; }

        [JsonPropertyName("lastActivityCompletedDate")]
        public DateTimeOffset LastActivityCompletedDate { get; set; }

        [JsonPropertyName("wasAbandoned")]
        public bool WasAbandoned { get; set; }

        [JsonPropertyName("hasCompleted")]
        public bool HasCompleted { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("categoryId")]
        public long CategoryId { get; set; }

        [JsonPropertyName("isTieredOrQuizMission")]
        public bool IsTieredOrQuizMission { get; set; }

        [JsonPropertyName("displayDescription")]
        public string DisplayDescription { get; set; }

        [JsonPropertyName("iconUri")]
        public Uri IconUri { get; set; }

        [JsonPropertyName("isParentMission")]
        public bool IsParentMission { get; set; }

        [JsonPropertyName("isStarterMission")]
        public bool IsStarterMission { get; set; }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("seasonId")]
        public long? SeasonId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
