using Microsoft.Xbox.Ambassadors.API;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors
{
    public static class AmbassadorProfile
    {
        public static async Task<bool> LoadAsync(CoreDispatcher dispatcher)
        {
            try
            {
                var token = await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false);
                UserIdentity = await Identity.GetAsync(token).ConfigureAwait(false);
                if (UserIdentity.IsBanned)
                {
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        await Windows.System.Launcher.LaunchUriAsync(new Uri("https://ambassadors.microsoft.com/xbox/access?removed=true"));
                    });
                    AccessToken.ClearVault();
                    return false;
                }
                if (!UserIdentity.IsAmbassador)
                {
                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        string message = "To access this application you must be an Xbox ambassador. You will be redirected to the Xbox Ambassadors website where you can join the program.";
                        MessageDialog md = new MessageDialog(message, "Just one more minute...");
                        await md.ShowAsync();

                        await Windows.System.Launcher.LaunchUriAsync(new Uri("https://ambassadors.microsoft.com/xbox"));
                    });
                    AccessToken.ClearVault();
                    return false;
                }
                Profile = await Profiles.GetAsync(token).ConfigureAwait(false);

                var s = await Season.GetCurrentAsync(token).ConfigureAwait(false);
                SeasonId = s.SeasonId;



                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }
        }

        public static async void UpdateProfile()
        {
            try
            {
                var token = await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false);
                Profile = await Profiles.GetAsync(token).ConfigureAwait(true);
            }
            catch
            {

            }
        }

        public static Identity UserIdentity { get; set; }
        public static Profiles Profile { get; set; }

        public static long SeasonId { get; set; }

    }
}
