using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public class Identity
    {
        const string REQUEST_URI = "https://ambassadors.westus.cloudapp.azure.com:8637/api/identity";

        public static async Task<Identity> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<Identity>(token, new Uri(REQUEST_URI)).ConfigureAwait(false);
        }

        [JsonPropertyName("isBanned")]
        public bool IsBanned { get; set; }

        [JsonPropertyName("isAmbassador")]
        public bool IsAmbassador { get; set; }

        [JsonPropertyName("ambassadorProfileId")]
        public string AmbassadorProfileId { get; set; }

        [JsonPropertyName("ambassadorWatchTowerContributorId")]
        public string AmbassadorWatchTowerContributorId { get; set; }

        [JsonPropertyName("isUserInTenantProgram")]
        public bool IsUserInTenantProgram { get; set; }

        [JsonPropertyName("bypassBackgroundCheck")]
        public bool BypassBackgroundCheck { get; set; }

        [JsonPropertyName("welcomeExperienceViewed")]
        public bool WelcomeExperienceViewed { get; set; }

        [JsonPropertyName("isAmbassadorLastUpdated")]
        public DateTimeOffset IsAmbassadorLastUpdated { get; set; }

        [JsonPropertyName("userDetails")]
        public UserDetails UserDetails { get; set; }

        [JsonPropertyName("backGroundCheckDetails")]
        public BackGroundCheckDetails BackGroundCheckDetails { get; set; }

        [JsonPropertyName("userAADGroups")]
        public object UserAadGroups { get; set; }

        [JsonPropertyName("hasSignedPledge")]
        public bool HasSignedPledge { get; set; }
    }
    public partial class BackGroundCheckDetails
    {
        [JsonPropertyName("isBackgroundCheckPass")]
        public bool IsBackgroundCheckPass { get; set; }

        [JsonPropertyName("backgroundCheckResults")]
        public List<BackgroundCheckResult> BackgroundCheckResults { get; set; }

        [JsonPropertyName("lastUpdated")]
        public DateTimeOffset LastUpdated { get; set; }
    }

    public partial class BackgroundCheckResult
    {
        [JsonPropertyName("requirementType")]
        public long RequirementType { get; set; }

        [JsonPropertyName("validationPassed")]
        public bool ValidationPassed { get; set; }

        [JsonPropertyName("actualValue")]
        public string ActualValue { get; set; }
    }

    public partial class UserDetails
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("displayPic")]
        public Uri DisplayPic { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("lastUpdated")]
        public DateTimeOffset LastUpdated { get; set; }
    }
}
