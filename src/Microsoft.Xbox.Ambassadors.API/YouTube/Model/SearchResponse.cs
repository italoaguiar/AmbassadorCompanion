using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API.YouTube.SearchResponseModels
{
    public class SearchResponse : Response
    {
        public Item[] items { get; set; }

        public override CommonItem[] GetItems()
        {
            return items;
        }
    }
    
    public class Id
    {
        [JsonPropertyName( "kind")]
        public string Kind { get; set; }

        [JsonPropertyName( "channelId")]
        public string ChannelId { get; set; }

        [JsonPropertyName( "videoId")]
        public string VideoId { get; set; }

        [JsonPropertyName( "playlistId")]
        public string PlaylistId { get; set; }
    }

    
    public class Thumbnail
    {
        [JsonPropertyName("url")]
        public Uri Url { get; set; }
    }

    
    public class Thumbnails
    {
        [JsonPropertyName( "default")]
        public Thumbnail Default { get; set; }

        [JsonPropertyName( "medium")]
        public Thumbnail Medium { get; set; }

        [JsonPropertyName( "high")]
        public Thumbnail High { get; set; }
    }

    
    public class Snippet
    {
        [JsonPropertyName( "publishedAt")]
        public DateTimeOffset PublishedAt { get; set; }

        [JsonPropertyName( "channelId")]
        public string ChannelId { get; set; }

        [JsonPropertyName( "title")]
        public string Title { get; set; }

        [JsonPropertyName( "description")]
        public string Description { get; set; }

        [JsonPropertyName( "thumbnails")]
        public Thumbnails Thumbnails { get; set; }

        [JsonPropertyName( "channelTitle")]
        public string ChannelTitle { get; set; }

        [JsonPropertyName( "liveBroadcastContent")]
        public string LiveBroadcastContent { get; set; }
    }

    
    public class Item:CommonItem
    {
        [JsonPropertyName( "snippet")]
        public Snippet Snippet { get; set; }

        [JsonPropertyName( "id")]
        public Id Id { get; set; }
    }



    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "toplevel", IsNullable = false)]
    public partial class SuggestionResponse
    {

        private toplevelCompleteSuggestion[] completeSuggestionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CompleteSuggestion")]
        public toplevelCompleteSuggestion[] CompleteSuggestion
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
