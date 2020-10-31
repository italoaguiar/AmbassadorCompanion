using Microsoft.Xbox.Ambassadors.API.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API
{
    public static class AmbassadorPreferences
    {
        const string REQUEST_URI = "https://ambassadors-production.azure-api.net/api/profiles/ambassadorpreferences/";

        public async static Task<bool> GetAsync(AccessToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            return await HttpUtil.GetAsync<bool>(token, new Uri(REQUEST_URI));
        }

        public async static Task<bool> PostAsync(AccessToken token, bool value)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            using (HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, new Uri(REQUEST_URI + value)))
            {
                msg.Headers.Add("Authorization", "Bearer " + token.Token);
                msg.Headers.Add("Accept", "application/json, text/plain, */*");
                using (HttpClient http = new HttpClient())
                {
                    HttpResponseMessage response = await http.SendAsync(msg);
                    return response.IsSuccessStatusCode;
                }
            }
        }
    }
}
