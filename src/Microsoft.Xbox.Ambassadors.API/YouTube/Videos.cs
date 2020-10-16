using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Microsoft.Xbox.Ambassadors.API.Auth;
using Microsoft.Xbox.Ambassadors.API.YouTube.Parameters;
using Microsoft.Xbox.Ambassadors.API.YouTube.VideoResponseModels;

namespace Microsoft.Xbox.Ambassadors.API.YouTube
{
    using InternetHelper = HttpUtil;

    public static class Videos
    {
        private const string requestUri = "https://www.googleapis.com/youtube/v3/videos";
        public static void Insert()
        {

        }
        public static async Task<VideosResponse> List(VideosParameters parameters)
        {
            return await List<VideosResponse>(parameters);
        }
        public static async Task<T> List<T>(VideosParameters parameters) where T : Response
        {
            return await InternetHelper.GetAsync<T>(requestUri, parameters);
        }
        public static async Task<VideosResponse> List(AccessToken token,VideosParameters parameters)
        {
            return await InternetHelper.GetAsync<VideosResponse>(token, new Uri(requestUri), parameters);
        }
        public static void Delete()
        {

        }

        public static void ReportAbuse()
        {

        }

        public enum Rating
        {
            like,dislike,none
        }
    }

}
