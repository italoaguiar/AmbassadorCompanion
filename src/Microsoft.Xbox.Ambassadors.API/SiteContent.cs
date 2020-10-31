using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public partial class SiteContent
    {
        const string REQUEST_URI =
            "https://ambassadors-production.azure-api.net/api/content?market=us&language=en-us&documentAliases=";

        public const string ANNOUNCEMENTS = "microsoft-ambassadors/xbox-ambassadors/dashboard-announcements";
        public const string FEATURED_MISSION = "microsoft-ambassadors/xbox-ambassadors/dashboard-featuredMission";
        public const string FEATURED_BLOG = "microsoft-ambassadors/xbox-ambassadors/dashboard-featuredBlog";
        public const string FEATURED_ACTIVITY = "microsoft-ambassadors/xbox-ambassadors/dashboard-featuredActivity";
        public const string AMBASSADOR_OF_MONTH = "microsoft-ambassadors/xbox-ambassadors/dashboard-ambOfMonth";
        public const string SOCIAL_ICONS = "microsoft-ambassadors/xbox-ambassadors/dashboard-socialIcons";
        public const string REWARDS_INTRO = "microsoft-ambassadors/xbox-ambassadors/rewards-intro";
        public const string REWARDS_MISSION_CARD = "microsoft-ambassadors/xbox-ambassadors/rewards-missionsCard";
        public const string SWEEPSTAKES_CARD = "microsoft-ambassadors/xbox-ambassadors/rewards-sweepstakesCard";
        public const string MY_REWARDS = "microsoft-ambassadors/xbox-ambassadors/rewards-myRewards";
        public const string HANDBOOK = "microsoft-ambassadors/xbox-ambassadors/handbook";
        public const string ACADEMY = "microsoft-ambassadors/xbox-ambassadors/academy-intro";

        public static async Task<SiteContent> GetAsync(AccessToken token, params string[] list)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (list == null)
                throw new ArgumentNullException(nameof(list));

            string req = REQUEST_URI;

            foreach(string s in list)
            {
                req += s + ",";
            }

            req = req.Substring(0, req.Length - 1);

            return await HttpUtil.GetAsync<SiteContent>(token, new Uri(req));
        }

        public DocumentElement this[string alias] => Documents.Where(x => x.Alias == alias).FirstOrDefault();


        [JsonPropertyName("documents")]
        public IList<DocumentElement> Documents { get; set; }

        [JsonPropertyName("errors")]
        public IList<object> Errors { get; set; }

        
        
    }

    public partial class DocumentElement
    {
        [JsonPropertyName("alias")]
        public string Alias { get; set; }

        [JsonPropertyName("document")]
        public Document Document { get; set; }
     
    }

    public partial class Document
    {
        [JsonPropertyName("_locale")]
        public string Locale { get; set; }

        [JsonPropertyName("_name")]
        public string Name { get; set; }

        [JsonPropertyName("sectionType")]
        public SectionType SectionType { get; set; }

        [JsonPropertyName("_lastEditedDateTime")]
        public string LastEditedDateTime { get; set; }

        [JsonPropertyName("_createdDateTime")]
        public string CreatedDateTime { get; set; }

        [JsonPropertyName("handbookSections")]
        public IList<HandbookSection> HandbookSections { get; set; }

        [JsonPropertyName("callToActionLink")]
        public CallToActionLink CallToActionLink { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("header")]
        public string Header { get; set; }
    }

    public partial class CallToActionLink
    {
        [JsonPropertyName("destinationUrl")]
        public Uri DestinationUrl { get; set; }

        [JsonPropertyName("linkText")]
        public string LinkText { get; set; }

        [JsonPropertyName("ariaLabel")]
        public string AriaLabel { get; set; }

        [JsonPropertyName("openInNewTab")]
        public bool OpenInNewTab { get; set; }
    }

    public partial class SectionType
    {
        [JsonPropertyName("featuredStories")]
        public FeaturedStories FeaturedStories { get; set; }

        [JsonPropertyName("featuredMission")]
        public FeaturedMission FeaturedMission { get; set; }

        [JsonPropertyName("featuredBlog")]
        public FeaturedBlog FeaturedBlog { get; set; }

        [JsonPropertyName("featuredActivity")]
        public FeaturedActivity FeaturedActivity { get; set; }

        [JsonPropertyName("ambassadorMonth")]
        public AmbassadorMonth AmbassadorMonth { get; set; }

        [JsonPropertyName("socialIcons")]
        public IList<SocialIcon> SocialIcons { get; set; }
    }

    public partial class AmbassadorMonth
    {
        [JsonPropertyName("dashboardBaseContent")]
        public DashboardBaseContent DashboardBaseContent { get; set; }

        [JsonPropertyName("subHeader")]
        public string SubHeader { get; set; }
    }

    public partial class DashboardBaseContent
    {
        [JsonPropertyName("header")]
        public string Header { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonPropertyName("imageAltText")]
        public string ImageAltText { get; set; }

        [JsonPropertyName("callToActionLink")]
        public OnLink CallToActionLink { get; set; }
    }

    public partial class OnLink
    {
        [JsonPropertyName("destinationUrl")]
        public string DestinationUrl { get; set; }

        [JsonPropertyName("linkText")]
        public string LinkText { get; set; }

        [JsonPropertyName("ariaLabel")]
        public string AriaLabel { get; set; }

        [JsonPropertyName("openInNewTab")]
        public bool OpenInNewTab { get; set; }
    }

    public partial class FeaturedActivity
    {
        [JsonPropertyName("dashboardBaseContent")]
        public DashboardBaseContent DashboardBaseContent { get; set; }

        [JsonPropertyName("sectionTitle")]
        public string SectionTitle { get; set; }
    }

    public partial class FeaturedBlog
    {
        [JsonPropertyName("dashboardBaseContent")]
        public DashboardBaseContent DashboardBaseContent { get; set; }

        [JsonPropertyName("publishDate")]
        public string PublishDate { get; set; }

        [JsonPropertyName("blogCategory")]
        public string BlogCategory { get; set; }

        [JsonPropertyName("authorUserName")]
        public string AuthorUserName { get; set; }

        [JsonPropertyName("authorImageUrl")]
        public Uri AuthorImageUrl { get; set; }

        [JsonPropertyName("authorImageAlt")]
        public string AuthorImageAlt { get; set; }
    }

    public partial class FeaturedMission
    {
        [JsonPropertyName("dashboardBaseContent")]
        public DashboardBaseContent DashboardBaseContent { get; set; }

        [JsonPropertyName("endDate")]
        public DateTimeOffset EndDate { get; set; }

        [JsonPropertyName("missionReward")]
        public string MissionReward { get; set; }
    }

    public partial class FeaturedStories
    {
        [JsonPropertyName("story1")]
        public Story Story1 { get; set; }

        [JsonPropertyName("story2")]
        public Story Story2 { get; set; }

        [JsonPropertyName("story3")]
        public Story Story3 { get; set; }
    }

    public partial class Story
    {
        [JsonPropertyName("dashboardBaseContent")]
        public DashboardBaseContent DashboardBaseContent { get; set; }
    }

    public partial class SocialIcon
    {
        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("iconLink")]
        public OnLink IconLink { get; set; }
    }


    public partial class HandbookSection
    {
        [JsonPropertyName("tabTitle")]
        public string TabTitle { get; set; }

        [JsonPropertyName("sectionTitle")]
        public string SectionTitle { get; set; }

        [JsonPropertyName("anchorTag")]
        public string AnchorTag { get; set; }

        [JsonPropertyName("sectionContent")]
        public IList<SectionContent> SectionContent { get; set; }

        [JsonPropertyName("subSection")]
        public IList<HandbookSection> SubSections { get; set; }
    }

    public partial class SectionContent
    {
        [JsonPropertyName("paragraphSection")]
        public ParagraphSection ParagraphSection { get; set; }

        [JsonPropertyName("listType")]
        public ListType? ListType { get; set; }

        [JsonPropertyName("listItemsArray")]
        public IList<ListItemsArray> ListItemsArray { get; set; }
    }

    public partial class ListItemsArray
    {
        [JsonPropertyName("listItem")]
        public string ListItem { get; set; }
    }

    public partial class ParagraphSection
    {
        [JsonPropertyName("paragraphContent")]
        public string ParagraphContent { get; set; }
    }

    public enum ListType { UnorderedList };
}
