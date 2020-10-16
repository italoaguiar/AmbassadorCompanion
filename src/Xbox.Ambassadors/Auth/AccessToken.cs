using System.Text.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Xbox.Ambassadors.Auth
{
    public class AccessToken : Microsoft.Xbox.Ambassadors.API.Auth.AccessToken
    {
        public void SaveInValt()
        {
            ExpirationTime = DateTime.Now.AddSeconds(ExpiresIn);

            string r = JsonSerializer.Serialize(this);

            PasswordVault vlt = new PasswordVault();


            vlt.Add(new PasswordCredential("XBA", "access_token", r));
        }

        private static PasswordVault Vault { get; set; } = new PasswordVault();

        public static async Task<AccessToken> LoadFromVaultOrGetNew()
        {
            if (!Vault.RetrieveAll().Any(x => x.Resource == "XBA"))
            {
                return await (new Authentication()).Login();
            }

            PasswordCredential p = Vault.Retrieve("XBA", "access_token");


            var t = JsonSerializer.Deserialize<AccessToken>(p.Password);



            if (t.ExpirationTime < DateTime.Now)
            {
                ClearVault();
                Authentication a = new Authentication();
                return await a.Login();

            }

            return t;
        }

        public static bool IsTokenValid
        {
            get
            {
                try
                {
                    PasswordVault vlt = new PasswordVault();
                    var tk = vlt.Retrieve("XBA", "access_token");
                    var t = JsonSerializer.Deserialize<AccessToken>(tk.Password);
                    return t.ExpirationTime > DateTime.Now;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool CheckTokenValidity()
        {
            return IsTokenValid;
        }

        public static void ClearVault()
        {
            PasswordVault vlt = new PasswordVault();
            foreach (var j in vlt.FindAllByResource("XBA"))
            {
                vlt.Remove(j);
            }
        }
    }
}
