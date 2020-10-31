using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace Xbox.Ambassadors.Mobile.Authentication
{
    public class Authenticator
    {

        private const string AUTH_URI = "https://login.live.com/oauth20_authorize.srf";
        private const string END_URI = "http://localhost/";

        private const string CLIENT_ID = "962b9d7d-85fe-4ac2-849c-81c097fe2643";
        private const string SCOPE = "Ambassadors.API";

        public void AuthenticateAsync()
        {
            var auth = new OAuth2Authenticator(CLIENT_ID, SCOPE, new Uri(AUTH_URI), new Uri(END_URI));

            auth.Completed += (s, a) =>
            {
                System.Diagnostics.Debug.WriteLine(a.Account);
                System.Diagnostics.Debug.WriteLine(a.IsAuthenticated);
            };


            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(auth);
        }
    }
}
