using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API.YouTube
{
    public class Key
    {
        private static string _publicKey;
        private static string _secretKey;
        private static string _clientId;

        public string PublicKey
        {
            get
            {
                return GetPublicKey();
            }
            set
            {
                _publicKey = value;
            }
        }
        public string SecretKey
        {
            get
            {
                return GetSecretKey();
            }
            set
            {
                _secretKey = value;
            }
        }
        public string ClientId
        {
            get
            {
                return GetClientId();
            }
            set
            {
                _clientId = value;
            }
        }
        public static string GetClientId()
        {
            if (string.IsNullOrEmpty(_clientId))
                throw new ArgumentNullException("Invalid Client Id");
            return _clientId;
        }
        public static string GetSecretKey()
        {
            if (string.IsNullOrEmpty(_secretKey))
                throw new ArgumentNullException("Invalid Secret Key");
            return _secretKey;
        }
        public static string GetPublicKey()
        {
            if (string.IsNullOrEmpty(_publicKey))
                throw new ArgumentNullException("Invalid Public Key");
            return _publicKey;
        }
    }
}
