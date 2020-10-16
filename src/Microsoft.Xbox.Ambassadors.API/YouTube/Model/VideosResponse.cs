using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Microsoft.Xbox.Ambassadors.API.YouTube.VideoResponseModels
{
    
    public class VideosResponse<T> : Response where T: Item
    {
        [JsonPropertyName( "items")]
        public virtual T[] Items { get; set; }

        public override CommonItem[] GetItems()
        {
            return Items;
        }
    }

    public class VideosResponse: VideosResponse<Item> { }

    
    public class Item:CommonItem
    {
        [JsonPropertyName( "id")]
        public string Id { get; set; }

        [JsonPropertyName( "snippet")]
        public Snippet Snippet { get; set; }

        [JsonPropertyName( "contentDetails")]
        public Contentdetails ContentDetails { get; set; }

        [JsonPropertyName( "status")]
        public Status Status { get; set; }

        [JsonPropertyName( "statistics")]
        public Statistics Statistics { get; set; }

        [JsonPropertyName( "player")]
        public Player Player { get; set; }

        [JsonPropertyName( "topicDetails")]
        public Topicdetails TopicDetails { get; set; }
    }

    
    public class Snippet:CommonSnippet
    {
        [JsonPropertyName( "categoryId")]
        public string CategoryId { get; set; }

        [JsonPropertyName( "liveBroadcastContent")]
        public string LiveBroadcastContent { get; set; }

        [JsonPropertyName( "localized")]
        public Localized Localized { get; set; }
    }

    
    public class Localized
    {
        [JsonPropertyName( "title")]
        public string Title { get; set; }

        [JsonPropertyName( "description")]
        public string Description { get; set; }
    }

    
    public class Contentdetails
    {
        [JsonPropertyName( "duration")]
        public string Duration { get; set; }

        [JsonPropertyName( "dimension")]
        public string Dimension { get; set; }

        [JsonPropertyName( "definition")]
        public string Definition { get; set; }

        [JsonPropertyName( "caption")]
        public string Caption { get; set; }

        [JsonPropertyName( "licensedContent")]
        public bool LicensedContent { get; set; }

        [JsonPropertyName( "regionRestriction")]
        public Regionrestriction RegionRestriction { get; set; }

        public string FormattedDuration
        {
            get
            {
                try
                {
                    TimeSpan t = XmlConvert.ToTimeSpan(Duration);
                    var time = t.ToString(@"mm\:ss");
                    return time;
                }
                catch
                {
                    return "00:00";
                }
            }
        }
    }

    
    public class Regionrestriction
    {
        [JsonPropertyName( "allowed")]
        public string[] Allowed { get; set; }
    }

    
    public class Status
    {
        [JsonPropertyName( "uploadStatus")]
        public string UploadStatus { get; set; }

        [JsonPropertyName( "privacyStatus")]
        public string PrivacyStatus { get; set; }

        [JsonPropertyName( "license")]
        public string License { get; set; }

        [JsonPropertyName( "embeddable")]
        public bool Embeddable { get; set; }

        [JsonPropertyName( "publicStatsViewable")]
        public bool PublicStatsViewable { get; set; }
    }

    
    public class Statistics
    {
        [JsonPropertyName( "viewCount")]
        public string ViewCount { get; set; }

        [JsonPropertyName( "likeCount")]
        public string LikeCount { get; set; }

        [JsonPropertyName( "dislikeCount")]
        public string DislikeCount { get; set; }

        [JsonPropertyName( "favoriteCount")]
        public string FavoriteCount { get; set; }

        [JsonPropertyName( "commentCount")]
        public string CommentCount { get; set; }

        public int TotalRatingCount
        {
            get
            {
                int lc = 0, dc = 0;
                bool r1 = int.TryParse(LikeCount, out lc);
                bool r2 = int.TryParse(DislikeCount, out dc);

                return lc + dc;
            }
        }
    }

    
    public class Player
    {
        [JsonPropertyName( "embedHtml")]
        public string EmbedHtml { get; set; }
    }

    
    public class Topicdetails
    {
        [JsonPropertyName( "topicIds")]
        public string[] TopicIds { get; set; }

        [JsonPropertyName( "relevantTopicIds")]
        public string[] RelevantTopicIds { get; set; }
    }



    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "toplevel", IsNullable = false)]
    public partial class SuggestionResponse
    {

        private toplevelCompleteSuggestion completeSuggestionField;

        /// <remarks/>
        public toplevelCompleteSuggestion CompleteSuggestion
        {
            get
            {
                return this.completeSuggestionField;
            }
            set
            {
                this.completeSuggestionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class toplevelCompleteSuggestion
    {

        private toplevelCompleteSuggestionSuggestion suggestionField;

        /// <remarks/>
        public toplevelCompleteSuggestionSuggestion suggestion
        {
            get
            {
                return this.suggestionField;
            }
            set
            {
                this.suggestionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class toplevelCompleteSuggestionSuggestion
    {

        private string dataField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }
    }


}
