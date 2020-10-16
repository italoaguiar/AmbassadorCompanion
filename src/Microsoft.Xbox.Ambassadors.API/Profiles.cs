using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public class Profiles:INotifyPropertyChanged
    {
        const string REQUEST_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/profiles/";

        

        public static async Task<Profiles> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Profiles>(token, new Uri(REQUEST_URI));
        }

        public static async Task<Profiles> GetAsync(string username)
        {
            return await HttpUtil.GetAsync<Profiles>(new Uri(REQUEST_URI + username));
        }

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("ambassadorProfileId")]
        public long AmbassadorProfileId { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("xuid")]
        public string Xuid { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("lastUpdatedDateTime")]
        public DateTimeOffset LastUpdatedDateTime { get; set; }

        [JsonPropertyName("gamertag")]
        public string Gamertag { get; set; }

        [JsonPropertyName("firstName")]
        public object FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public object LastName { get; set; }

        [JsonPropertyName("watchtowerContributorId")]
        public Guid WatchtowerContributorId { get; set; }

        [JsonPropertyName("bypassBackgroundCheck")]
        public bool BypassBackgroundCheck { get; set; }

        [JsonPropertyName("isBanned")]
        public bool IsBanned { get; set; }

        [JsonPropertyName("version")]
        public long Version { get; set; }

        [JsonPropertyName("totalXp")]
        public long TotalXp { get; set; }

        [JsonPropertyName("totalActivitiesCompleted")]
        public long TotalActivitiesCompleted { get; set; }

        [JsonPropertyName("currentTier")]
        public CurrentTier CurrentTier { get; set; }

        [JsonPropertyName("nextTierLabel")]
        public string NextTierLabel { get; set; }

        [JsonPropertyName("nextTierId")]
        public int? NextTierId { get; set; }

        [JsonPropertyName("highestTierEarned")]
        public CurrentTier HighestTierEarned { get; set; }

        [JsonPropertyName("badges")]
        public IList<Badge> Badges { get; set; }


        [JsonPropertyName("seasonStatistics")]
        public IList<SeasonStatistic> SeasonStatistics { get; set; }

        [JsonPropertyName("joinedDateTime")]
        public DateTimeOffset JoinedDateTime { get; set; }

        [JsonPropertyName("legacyJoinedDateTime")]
        public object LegacyJoinedDateTime { get; set; }

        [JsonPropertyName("lastRewardEarnedDateTime")]
        public DateTimeOffset LastRewardEarnedDateTime { get; set; }

        [JsonPropertyName("lastRewardEarnedDefinitionId")]
        public long LastRewardEarnedDefinitionId { get; set; }

        [JsonPropertyName("lastRewardEarnedTrophyURL")]
        public Uri LastRewardEarnedTrophyUrl { get; set; }

        [JsonPropertyName("ambassadorBadgeUrl")]
        public string AmbassadorBadgeUrl { get; set; }

        [JsonPropertyName("recentBadgesPins")]
        public IList<Badge> RecentBadgesPins { get; set; }

        [JsonPropertyName("recentActivities")]
        public IList<Recent> RecentActivities { get; set; }

        [JsonPropertyName("recentReactions")]
        public IList<Recent> RecentReactions { get; set; }

        [JsonPropertyName("dynamicBadges")]
        public IList<DynamicBadge> DynamicBadges { get; set; }

        [JsonPropertyName("hasSignedPledge")]
        public bool HasSignedPledge { get; set; }

        [JsonPropertyName("pledgeSignedDate")]
        public DateTimeOffset PledgeSignedDate { get; set; }

        [JsonPropertyName("currentLevel")]
        public Level CurrentLevel { get; set; }

        [JsonPropertyName("hitNewLevel")]
        public bool HitNewLevel { get; set; }

        private long availableSweepstakesTickets;

        [JsonPropertyName("availableSweepstakeTickets")]
        public long AvailableSweepstakeTickets
        {
            get => availableSweepstakesTickets;
            set
            {
                availableSweepstakesTickets = value;
                OnPropertyChanged(nameof(AvailableSweepstakeTickets));
            }
        }

        [JsonPropertyName("optedInToDisplayAmbBadge")]
        public bool OptedInToDisplayAmbBadge { get; set; }


        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


    public class Badge
    {
        [JsonPropertyName("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("subTitle")]
        public string SubTitle { get; set; }

        [JsonPropertyName("rewardDefinitionId")]
        public long RewardDefinitionId { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("rewardTypeId")]
        public long RewardTypeId { get; set; }
    }

    public class Level
    {
        [JsonPropertyName("levelNumber")]
        public long LevelNumber { get; set; }

        [JsonPropertyName("longName")]
        public string LongName { get; set; }

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; }

        [JsonPropertyName("levelBadgeUrl")]
        public Uri LevelBadgeUrl { get; set; }

        [JsonPropertyName("levelHudBadgeUrl")]
        public Uri LevelHudBadgeUrl { get; set; }

        [JsonPropertyName("minXP")]
        public long MinXp { get; set; }

        [JsonPropertyName("maxXP")]
        public long MaxXp { get; set; }

        [JsonPropertyName("xpModifierDisplay")]
        public string XpModifierDisplay { get; set; }
    }

    public class CurrentTier
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("tierId")]
        public long? TierId { get; set; }

        [JsonPropertyName("xpEarned")]
        public long XpEarned { get; set; }

        [JsonPropertyName("xpNeededForNextTier")]
        public long? XpNeededForNextTier { get; set; }

        [JsonPropertyName("tierImg")]
        public Uri TierImg { get; set; }

        [JsonPropertyName("isTopTier")]
        public bool IsTopTier { get; set; }
    }

    public class DynamicBadge
    {
        [JsonPropertyName("dynamicBadgeDefinitionId")]
        public long DynamicBadgeDefinitionId { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("displayDescription")]
        public string DisplayDescription { get; set; }

        [JsonPropertyName("xpThreshold")]
        public long XpThreshold { get; set; }

        [JsonPropertyName("iconUri")]
        public Uri IconUri { get; set; }
    }

    public class Recent
    {
        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonPropertyName("activityList")]
        public IList<ActivityList> ActivityList { get; set; }
    }

    public class ActivityList
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
        public object Payload { get; set; }

        [JsonPropertyName("allowOneClick")]
        public bool AllowOneClick { get; set; }
    }

    public class SeasonStatistic
    {
        [JsonPropertyName("seasonName")]
        public string SeasonName { get; set; }

        [JsonPropertyName("seasonId")]
        public int SeasonId { get; set; }

        [JsonPropertyName("startDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonPropertyName("xpEarned")]
        public long XpEarned { get; set; }

        [JsonPropertyName("lastActivityDate")]
        public DateTimeOffset LastActivityDate { get; set; }

        [JsonPropertyName("totalActivitiesCompleted")]
        public long TotalActivitiesCompleted { get; set; }

        [JsonPropertyName("completedMissionsCount")]
        public long CompletedMissionsCount { get; set; }

        [JsonPropertyName("tierAchieved")]
        public string TierAchieved { get; set; }

        [JsonPropertyName("tierAchievedId")]
        public long? TierAchievedId { get; set; }

        [JsonPropertyName("channels")]
        public IList<Channel> Channels { get; set; }
    }

    public class Channel
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("channelId")]
        public long ChannelId { get; set; }

        [JsonPropertyName("iconUri")]
        public Uri IconUri { get; set; }

        [JsonPropertyName("xpEarned")]
        public long XpEarned { get; set; }

        [JsonPropertyName("completedActivityCount")]
        public long CompletedActivityCount { get; set; }

        [JsonPropertyName("lastActivityDate")]
        public DateTimeOffset? LastActivityDate { get; set; }
    }
}
