using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xbox.Ambassadors.API.Auth;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Encodings.Web;
using Microsoft.Xbox.Ambassadors.API.YouTube;

namespace Microsoft.Xbox.Ambassadors.API
{
    internal static class HttpUtil
    {
        const string UA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";


        

        private static T ParseJson<T>(string content, bool useDataContract = false)
        {
            if (useDataContract)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                {
                    return (T)ser.ReadObject(ms);
                }
            }
            else
            {
                var settings = new JsonSerializerOptions();
                settings.Converters.Add(new JsonStringEnumConverter());
                return JsonSerializer.Deserialize<T>(content, settings);
            }
        }
        public async static Task<T> RequestJsonAsync<T>(HttpRequestMessage httpMessage, bool useDataContract = false)
        {
            using (HttpClient http = new HttpClient())
            {
                using (HttpResponseMessage response = await http.SendAsync(httpMessage).ConfigureAwait(false))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        System.Diagnostics.Debug.WriteLine(httpMessage.RequestUri);
                        System.Diagnostics.Debug.WriteLine(content);
                        throw new RequestException(response.StatusCode)
                        {
                            RequestUri = httpMessage.RequestUri,
                            ResponseContent = content
                        };
                    }
                    try
                    {
                        return ParseJson<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false), useDataContract);
                    }
                    catch (Exception e)
                    {
                        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        System.Diagnostics.Debug.WriteLine(httpMessage.RequestUri);
                        System.Diagnostics.Debug.WriteLine(content);
                        throw new RequestException(response.StatusCode, e.Message, e)
                        {
                            RequestUri = httpMessage.RequestUri,
                            ResponseContent = content
                        };
                    }
                }
            }
        }

        public async static Task<T> GetAsync<T>(Uri url)
        {
            using (var msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                return await RequestJsonAsync<T>(msg).ConfigureAwait(false);
            }
        }

        public async static Task<T> GetAsync<T>(string url, object args)
        {
            var uri = $"{url}{args}key={Key.GetPublicKey()}";

            return await GetAsync<T>(new Uri(uri)).ConfigureAwait(false);
        }
        

        public async static Task<T> GetAsync<T>(AccessToken token, Uri url, string body, string contentType = "application/json")
        {
            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                msg.Headers.Add("User-Agent", UA);
                msg.Content = new StringContent(body);
                msg.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                return await RequestJsonAsync<T>(msg).ConfigureAwait(false);
            }
        }

        public async static Task<T> GetAsync<T>(AccessToken token, Uri url, object data)
        {
            return await GetAsync<T>(token, url, JsonSerializer.Serialize(data)).ConfigureAwait(false);
        }

        public async static Task<T> GetAsync<T>(AccessToken token, Uri url, Dictionary<string, string> headers = null)
        {
            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, url))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                msg.Headers.Add("User-Agent", UA);

                if (headers != null)
                    foreach (var h in headers)
                    {
                        msg.Headers.Add(h.Key, h.Value);
                    }

                return await RequestJsonAsync<T>(msg).ConfigureAwait(false);
            }
        }


        public async static Task<T> PostAsync<T>(AccessToken token, Uri url, string body, string contentType = "application/json", Dictionary<string,string> headers = null)
        {
            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                msg.Headers.Add("User-Agent", UA);

                if (headers != null)
                {
                    foreach (var h in headers)
                    {
                        msg.Headers.Add(h.Key, h.Value);
                    }
                }

                msg.Content = new StringContent(body);
                msg.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                return await RequestJsonAsync<T>(msg).ConfigureAwait(false);
            }
        }

        public async static Task<T> PostAsync<T>(AccessToken token, Uri url, object data)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return await PostAsync<T>(token, url, JsonSerializer.Serialize(data, options)).ConfigureAwait(false);
        }

        public async static Task<T> PostAsync<T>(AccessToken token, Uri url, object data, Dictionary<string,string> headers)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return await PostAsync<T>(token, url, JsonSerializer.Serialize(data, options), "application/json", headers).ConfigureAwait(false);
        }

        public async static Task<T> PostAsync<T>(AccessToken token, Uri url)
        {
            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, url))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                msg.Headers.Add("User-Agent", UA);

                return await RequestJsonAsync<T>(msg).ConfigureAwait(false);
            }
        }


        public async static Task<HttpResponseMessage> DeleteAsync(AccessToken token, Uri url, object data)
        {
            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                msg.Headers.Add("host", url.Authority);
                msg.Headers.Add("User-Agent", UA);
                msg.Content = new StringContent(JsonSerializer.Serialize(data));
                msg.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                using (HttpClient http = new HttpClient())
                {
                    return await http.SendAsync(msg).ConfigureAwait(false);
                }
            }
        }
    }
}
