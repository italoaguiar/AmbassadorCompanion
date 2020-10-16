using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API.YouTube.Parameters
{
    public class SearchParameters:Parameter
    {
        /// <summary>
        /// The part parameter specifies a comma-separated list of one or 
        /// more search resource properties that the API response will 
        /// include. Set the parameter value to snippet. 
        /// </summary>
        public string part { get; set; } = "snippet";

        /// <summary>
        /// This parameter can only be used in a properly authorized request. 
        /// Note: This parameter is intended exclusively for YouTube content 
        /// partners.The forContentOwner parameter restricts the search to only 
        /// retrieve resources owned by the content owner specified by the 
        /// onBehalfOfContentOwner parameter. The user must be authenticated 
        /// using a CMS account linked to the specified content owner and 
        /// onBehalfOfContentOwner must be provided
        /// </summary>
        public string forContentOwner { get; set; }

        /// <summary>
        /// This parameter can only be used in a properly authorized request. 
        /// The forDeveloper parameter restricts the search to only retrieve 
        /// videos uploaded via the developer's application or website. 
        /// The API server uses the request's authorization credentials to 
        /// identify the developer. The forDeveloper parameter can be used in 
        /// conjunction with optional search parameters like the q parameter.
        /// </summary>
        public string forDeveloper { get; set; }

        /// <summary>
        /// This parameter can only be used in a properly authorized request. 
        /// The forMine parameter restricts the search to only retrieve videos 
        /// owned by the authenticated user. If you set this parameter to true, 
        /// then the type parameter's value must also be set to video.
        /// </summary>
        public string forMine { get; set; }

        /// <summary>
        /// The relatedToVideoId parameter retrieves a list of videos that are 
        /// related to the video that the parameter value identifies. The parameter 
        /// value must be set to a YouTube video ID and, if you are using this 
        /// parameter, the type parameter must be set to video.
        /// </summary>
        public string relatedToVideoId { get; set; }



        /*************************************************************  
         * 
         * OPTIONAL PARAMETERS 
         *
         *************************************************************/

        /// <summary>
        /// The channelId parameter indicates that the API response should only 
        /// contain resources created by the channel
        /// </summary>
        public string channelId { get; set; }

        /// <summary>
        /// The channelType parameter lets you restrict a search to a particular type of channel.
        /// 
        /// any – Return all channels.
        /// show – Only retrieve shows.
        /// </summary>
        public string channelType { get; set; }

        public enum ChannelType
        {
            any, show
        }

        /// <summary>
        /// The eventType parameter restricts a search to broadcast events. If you specify a 
        /// value for this parameter, you must also set the type parameter's value to video.
        /// 
        /// completed – Only include completed broadcasts.
        /// live – Only include active broadcasts.
        /// upcoming – Only include upcoming broadcasts.
        /// </summary>
        public string eventType { get; set; }

        public enum EventType
        {
            completed, live, upcoming
        }

        /// <summary>
        /// The location parameter, in conjunction with the locationRadius parameter, 
        /// defines a circular geographic area and also restricts a search to videos 
        /// that specify, in their metadata, a geographic location that falls within 
        /// that area. The parameter value is a string that specifies latitude/longitude 
        /// coordinates e.g. (37.42307,-122.08427).
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// The locationRadius parameter, in conjunction with the location parameter, 
        /// defines a circular geographic area. The parameter value must be a floating 
        /// point number followed by a measurement unit. Valid measurement units are 
        /// m, km, ft, and mi. For example, valid parameter values include 
        /// 1500m, 5km, 10000ft, and 0.75mi. The API does not support locationRadius 
        /// parameter values larger than 1000 kilometers.
        /// </summary>
        public string locationRadius { get; set; }


        private uint _maxResults = 25;
        /// <summary>
        /// The maxResults parameter specifies the maximum number of items that should 
        /// be returned in the result set. Acceptable values are 0 to 50, inclusive. 
        /// The default value is 25.
        /// </summary>
        public uint maxResults { get { return _maxResults; } set { _maxResults = value; } }

        /// <summary>
        /// This parameter can only be used in a properly authorized request. 
        /// Note: This parameter is intended exclusively for YouTube content partners.
        /// The onBehalfOfContentOwner parameter indicates that the request's 
        /// authorization credentials identify a YouTube CMS user who is acting on 
        /// behalf of the content owner specified in the parameter value. This parameter 
        /// is intended for YouTube content partners that own and manage many different 
        /// YouTube channels. It allows content owners to authenticate once and get access 
        /// to all their video and channel data, without having to provide authentication 
        /// credentials for each individual channel. The CMS account that the user 
        /// authenticates with must be linked to the specified YouTube content owner.
        /// </summary>
        public string onBehalfOfContentOwner { get; set; }

        /// <summary>
        /// The order parameter specifies the method that will be used to order 
        /// resources in the API response. The default value is relevance.
        /// </summary>
        public OrderType order { get; set; }

        public enum OrderType
        {
            relevance, date, rating, title, videoCount, viewCount
        }

        /// <summary>
        /// The publishedAfter parameter indicates that the API response should only 
        /// contain resources created after the specified time.
        /// </summary>
        public string publishedAfter { get; set; }

        /// <summary>
        /// The publishedBefore parameter indicates that the API response should only 
        /// contain resources created before the specified time.
        /// </summary>
        public string publishedBefore { get; set; }

        /// <summary>
        /// The q parameter specifies the query term to search for.
        /// </summary>
        public string q { get; set; }

        /// <summary>
        /// The regionCode parameter instructs the API to return search results for 
        /// the specified country. The parameter value is an ISO 3166-1 alpha-2 country code.
        /// </summary>
        public string regionCode { get; set; }

        /// <summary>
        /// The relevanceLanguage parameter instructs the API to return search results that 
        /// are most relevant to the specified language. The parameter value is typically an 
        /// ISO 639-1 two-letter language code. However, you should use the values zh-Hans 
        /// for simplified Chinese and zh-Hant for traditional Chinese. Please note that 
        /// results in other languages will still be returned if they are highly relevant 
        /// to the search query term.
        /// </summary>
        public string relevanceLanguage { get; set; }

        /// <summary>
        /// The safeSearch parameter indicates whether the search results should include 
        /// restricted content as well as standard content.
        /// </summary>
        public SafeSearch safeSearch { get; set; }

        public enum SafeSearch
        {
            moderate, strict, none
        }

        /// <summary>
        /// The topicId parameter indicates that the API response should only contain resources 
        /// associated with the specified topic. The value identifies a Freebase topic ID.
        /// </summary>
        public string topicId { get; set; }

        /// <summary>
        /// The type parameter restricts a search query to only retrieve a particular type 
        /// of resource. The value is a comma-separated list of resource types. The default 
        /// value is 'video,channel,playlist'.
        /// 
        /// Acceptable values are:
        /// channel
        /// playlist
        /// video
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// The videoCaption parameter indicates whether the API should filter video search 
        /// results based on whether they have captions. If you specify a value for this 
        /// parameter, you must also set the type parameter's value to video.
        /// </summary>
        public string videoCaption { get; set; }

        public enum VideoCaption
        {
            any, closedCaption, none
        }

        /// <summary>
        /// The videoCategoryId parameter filters video search results based on their category. 
        /// If you specify a value for this parameter, you must also set the type parameter's 
        /// value to video.
        /// </summary>
        public string videoCategoryId { get; set; }

        /// <summary>
        /// The videoDefinition parameter lets you restrict a search to only include either 
        /// definition (HD) or standard definition (SD) videos. HD videos are available for 
        /// playback in at least 720p, though higher resolutions, like 1080p, might also be 
        /// available. If you specify a value for this parameter, you must also set the type 
        /// parameter's value to video.
        /// </summary>
        public string videoDefinition { get; set; }

        public enum VideoDefinition
        {
            any, high, standard
        }

        /// <summary>
        /// The videoDimension parameter lets you restrict a search to only retrieve 2D or 3D 
        /// videos. If you specify a value for this parameter, you must also set the type 
        /// parameter's value to video.
        ///
        ///Acceptable values are:
        ///2d – Restrict search results to exclude 3D videos.
        ///3d – Restrict search results to only include 3D videos.
        ///any – Include both 3D and non-3D videos in returned results. This is the default value.
        /// </summary>
        public string videoDimension { get; set; }

        /// <summary>
        /// The videoDuration parameter filters video search results based on their duration. 
        /// If you specify a value for this parameter, you must also set the type parameter's 
        /// value to video.
        ///
        ///Acceptable values are:
        ///any – Do not filter video search results based on their duration. This is the default value.
        ///long – Only include videos longer than 20 minutes.
        ///medium – Only include videos that are between four and 20 minutes long (inclusive).
        ///short – Only include videos that are less than four minutes long.
        /// </summary>
        public string videoDuration { get; set; }

        /// <summary>
        /// The videoEmbeddable parameter lets you to restrict a search to only videos that can 
        /// be embedded into a webpage. If you specify a value for this parameter, you must also 
        /// set the type parameter's value to video.
        ///
        ///Acceptable values are:
        ///any – Return all videos, embeddable or not.
        ///true – Only retrieve embeddable videos.
        /// </summary>
        public string videoEmbeddable { get; set; }

        /// <summary>
        /// The videoLicense parameter filters search results to only include 
        /// videos with a particular license. YouTube lets video uploaders 
        /// choose to attach either the Creative Commons license or the standard 
        /// YouTube license to each of their videos. If you specify a value for 
        /// this parameter, you must also set the type parameter's value to video.
        /// </summary>
        public string videoLicense { get; set; }

        public enum VideoLicense
        {
            any, creativeCommon, youtube
        }


        /// <summary>
        /// The videoSyndicated parameter lets you to restrict a search to 
        /// only videos that can be played outside youtube.com. If you specify 
        /// a value for this parameter, you must also set the type parameter's 
        /// value to video.
        ///
        ///Acceptable values are:
        ///any – Return all videos, syndicated or not.
        ///true – Only retrieve syndicated videos.
        /// </summary>
        public string videoSyndicated { get; set; }

        /// <summary>
        /// The videoType parameter lets you restrict a search to a particular 
        /// type of videos. If you specify a value for this parameter, you must 
        /// also set the type parameter's value to video.
        /// </summary>
        public string videoType { get; set; }

        public enum VideoType
        {
            any, episode, movie
        }

        public override string ToString()
        {
            string c = "?";
            foreach (PropertyInfo p in this.GetType().GetRuntimeProperties())
            {
                if (p.GetValue(this, null) != null)
                {
                    c += string.Format("{0}={1}&", p.Name, p.GetValue(this, null));
                }
            }
            return c;
        }
    }
}
