using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API.Auth
{
    public class AccessToken
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string Scope { get; set; }

        public AccessToken() { }

        public AccessToken(string token, string type, int expires, string scope)
        {
            Token = token;
            TokenType = type;
            ExpiresIn = expires;
            Scope = scope;
        }
    }
}
