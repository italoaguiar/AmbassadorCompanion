using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API.Blog
{
    public static class Blog
    {

        private const string REQUEST_URI = "https://blog-ambassadors.xbox.com/wp-json/wp/v2/posts?per_page=5";

        public async static Task<IList<Post>> GetPostsAsync()
        {
            return await HttpUtil.GetAsync<IList<Post>>(new Uri(REQUEST_URI)).ConfigureAwait(false);
        }
    }

    public class PostGuid
    {

        [JsonPropertyName("rendered")]
        public string Rendered { get; set; }
    }

    public class Title
    {

        [JsonPropertyName("rendered")]
        public string Rendered { get; set; }
    }

    public class Content
    {

        [JsonPropertyName("rendered")]
        public string Rendered { get; set; }

        [JsonPropertyName("protected")]
        public bool Protected { get; set; }
    }

    public class Excerpt
    {

        [JsonPropertyName("rendered")]
        public string Rendered { get; set; }

        [JsonPropertyName("protected")]
        public bool Protected { get; set; }
    }

    public class Meta
    {

        [JsonPropertyName("spay_email")]
        public string SpayEmail { get; set; }
    }

    public class Self
    {

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class Collection
    {

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class About
    {

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class Author
    {

        [JsonPropertyName("embeddable")]
        public bool Embeddable { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class Reply
    {

        [JsonPropertyName("embeddable")]
        public bool Embeddable { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class VersionHistory
    {

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class PredecessorVersion
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class WpFeaturedmedia
    {

        [JsonPropertyName("embeddable")]
        public bool Embeddable { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class WpAttachment
    {

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class WpTerm
    {

        [JsonPropertyName("taxonomy")]
        public string Taxonomy { get; set; }

        [JsonPropertyName("embeddable")]
        public bool Embeddable { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class Cury
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonPropertyName("templated")]
        public bool Templated { get; set; }
    }

    public class Links
    {

        [JsonPropertyName("self")]
        public IList<Self> Self { get; set; }

        [JsonPropertyName("collection")]
        public IList<Collection> Collection { get; set; }

        [JsonPropertyName("about")]
        public IList<About> About { get; set; }

        [JsonPropertyName("author")]
        public IList<Author> Author { get; set; }

        [JsonPropertyName("replies")]
        public IList<Reply> Replies { get; set; }

        [JsonPropertyName("version-history")]
        public IList<VersionHistory> VersionHistory { get; set; }

        [JsonPropertyName("predecessor-version")]
        public IList<PredecessorVersion> PredecessorVersion { get; set; }

        [JsonPropertyName("wp:featuredmedia")]
        public IList<WpFeaturedmedia> WpFeaturedmedia { get; set; }

        [JsonPropertyName("wp:attachment")]
        public IList<WpAttachment> WpAttachment { get; set; }

        [JsonPropertyName("wp:term")]
        public IList<WpTerm> WpTerm { get; set; }

        [JsonPropertyName("curies")]
        public IList<Cury> Curies { get; set; }
    }

    public class Post
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("date_gmt")]
        public DateTime DateGmt { get; set; }

        [JsonPropertyName("guid")]
        public PostGuid Guid { get; set; }

        [JsonPropertyName("modified")]
        public DateTime Modified { get; set; }

        [JsonPropertyName("modified_gmt")]
        public DateTime ModifiedGmt { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("link")]
        public Uri Link { get; set; }

        [JsonPropertyName("title")]
        public Title Title { get; set; }

        [JsonPropertyName("content")]
        public Content Content { get; set; }

        [JsonPropertyName("excerpt")]
        public Excerpt Excerpt { get; set; }

        [JsonPropertyName("author")]
        public int Author { get; set; }

        [JsonPropertyName("featured_media")]
        public int FeaturedMedia { get; set; }

        [JsonPropertyName("comment_status")]
        public string CommentStatus { get; set; }

        [JsonPropertyName("ping_status")]
        public string PingStatus { get; set; }

        [JsonPropertyName("sticky")]
        public bool Sticky { get; set; }

        [JsonPropertyName("template")]
        public string Template { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("categories")]
        public IList<int> Categories { get; set; }

        [JsonPropertyName("tags")]
        public IList<int> Tags { get; set; }

        [JsonPropertyName("jetpack_featured_media_url")]
        public Uri JetpackFeaturedMediaUrl { get; set; }

        [JsonPropertyName("jetpack_sharing_enabled")]
        public bool JetpackSharingEnabled { get; set; }

        [JsonPropertyName("jetpack_shortlink")]
        public string JetpackShortlink { get; set; }

        [JsonPropertyName("_links")]
        public Links Links { get; set; }
    }







}
