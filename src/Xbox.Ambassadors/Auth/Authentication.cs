using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace Xbox.Ambassadors.Auth
{
    public class Authentication
    {
        private const string AUTH_URI = "https://login.live.com/oauth20_authorize.srf?client_id=962b9d7d-85fe-4ac2-849c-81c097fe2643&scope=Ambassadors.API&response_type=token&redirect_uri=http://localhost/";
        private const string END_URI = "http://localhost/";

        public async Task<AccessToken> Login()
        {
            Uri startUri = new Uri(AUTH_URI);
            Uri endUri = new Uri(END_URI);


            WebAuthenticationResult webAuthenticationResult =
                     await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);



            if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                string response = webAuthenticationResult.ResponseData;

                Uri result = null;

                bool r = Uri.TryCreate(response, UriKind.Absolute, out result);
                if (r)
                {
                    var parameters = GetUriParameters(result);

                    AccessToken res = new AccessToken();
                    res.Token = Uri.UnescapeDataString(parameters["access_token"]);
                    res.TokenType = parameters["token_type"];
                    res.ExpiresIn = int.Parse(parameters["expires_in"]);
                    res.Scope = parameters["scope"];

                    res.SaveInValt();

                    return res;
                }
                else
                {
                    throw new System.Net.Http.HttpRequestException("Failed to Connect to Login WebService.");
                }

            }
            else if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                throw new System.Net.Http.HttpRequestException("Failed to Connect to Login WebService");
            }
            else if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.UserCancel)
            {
                throw new OperationCanceledException("The user canceled the login attempt.");
            }
            else
            {
                throw new Exception("Unknown Error");
            }
        }
        private Dictionary<string, string> GetUriParameters(Uri uri)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            Regex reg = new Regex(@"([^=&#\s]*)");

            var r = reg.Matches(uri.ToString());

            for (int i = 2; i < r.Count; i = i + 4)
            {
                if (i + 1 < r.Count)
                {
                    dic.Add(r[i].Value, r[i + 2].Value);
                }
            }
            return dic;
        }
    }
}
