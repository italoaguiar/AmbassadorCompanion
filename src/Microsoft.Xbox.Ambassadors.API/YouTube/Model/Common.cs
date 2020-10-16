using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API.YouTube
{
    
    public abstract class Response
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName( "etag")]
        public string ETag { get; set; }

        [JsonPropertyName( "nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonPropertyName( "pageInfo")]
        public PageInfo PageInfo { get; set; }

        public abstract CommonItem[] GetItems();
    }

    
    public class PageInfo
    {
        [JsonPropertyName( "totalResults")]
        public int TotalResults { get; set; }

        [JsonPropertyName( "resultsPerPage")]
        public int ResultsPerPage { get; set; }
    }

    
    public class CommonItem
    {
        [JsonPropertyName( "kind")]
        public string Kind { get; set; }

        [JsonPropertyName( "etag")]
        public string ETag { get; set; }        
    }

    
    public class CommonSnippet
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
    }

    
    public class Thumbnails
    {
        [JsonPropertyName( "default")]
        public Thumbnail Default { get; set; }

        [JsonPropertyName( "medium")]
        public Thumbnail Medium { get; set; }

        [JsonPropertyName( "high")]
        public Thumbnail High { get; set; }

        [JsonPropertyName( "standard")]
        public Thumbnail Standard { get; set; }

        [JsonPropertyName( "maxres")]
        public Thumbnail MaxResolution { get; set; }
    }

    
    public class Thumbnail
    {
        [JsonPropertyName( "url")]
        public Uri Url { get; set; }

        [JsonPropertyName( "width")]
        public int Width { get; set; }

        [JsonPropertyName( "height")]
        public int Height { get; set; }
    }

    
    public class CommentSnippet
    {
        [JsonPropertyName( "authorDisplayName")]
        public string AuthorDisplayName { get; set; }

        [JsonPropertyName( "authorProfileImageUrl")]
        public Uri AuthorProfileImageUrl { get; set; }

        [JsonPropertyName( "authorChannelUrl")]
        public Uri AuthorChannelUrl { get; set; }

        [JsonPropertyName( "authorChannelId.value")]
        public string AuthorChannelId { get; set; }

        [JsonPropertyName( "textDisplay")]
        public string TextDisplay { get; set; }

        [JsonPropertyName( "textOriginal")]
        public string TextOriginal { get; set; }

        [JsonPropertyName( "parentId")]
        public string ParentId { get; set; }

        [JsonPropertyName( "canRate")]
        public bool CanRate { get; set; }

        [JsonPropertyName( "viewerRating")]
        public string ViewerRating { get; set; }

        [JsonPropertyName( "likeCount")]
        public int LikeCount { get; set; }

        [JsonPropertyName( "publishedAt")]
        public string PublishedAt { get; set; }

        [JsonPropertyName( "updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName( "videoId")]
        public string VideoId { get; set; }
    }
}
