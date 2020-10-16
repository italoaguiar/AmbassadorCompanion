using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.Xbox.Ambassadors.API.YouTube.Parameters;
using Microsoft.Xbox.Ambassadors.API.Auth;
using Microsoft.Xbox.Ambassadors.API.YouTube.SearchResponseModels;

namespace Microsoft.Xbox.Ambassadors.API.YouTube
{
    using InternetHelper = HttpUtil;

    public static class Search
    {
        private const string requestUri = "https://www.googleapis.com/youtube/v3/search";

        public static async Task<SearchResponse> List(AccessToken token, SearchParameters parameters)
        {
            return await InternetHelper.GetAsync<SearchResponse>(token, new Uri(requestUri), parameters);
        }

        public static async Task<SearchResponse> List(SearchParameters parameters)
        {
            return await InternetHelper.GetAsync<SearchResponse>(requestUri, parameters);
        }

        public static async Task<List<string>> Suggest(string term)
        {
            try
            {
                HttpClient http = new System.Net.Http.HttpClient();
                Uri url = new Uri(string.Format("http://suggestqueries.google.com/complete/search?client=toolbar&ds=yt&q={0}", term));
                HttpResponseMessage response = await http.GetAsync(url);
                List<string> result = new List<string>();

                XmlReader reader = XmlReader.Create(new StringReader(await response.Content.ReadAsStringAsync()));
                while (reader.Read())
                {
                    if (reader.Name == "suggestion")
                    {
                        result.Add(reader["data"]);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                throw new RequestException(e.Message);
            }
        }
    }

}
