using System.Collections.Generic;
using Windows.Storage;

namespace Xbox.Ambassadors
{
    public static class AppSettings
    {
        static ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static bool ShowResponderNotifications
        {
            get
            {
                var r = localSettings.Values["ShowResponderNotifications"];

                return r == null ? true : (bool)r;
            }
            set
            {
                localSettings.Values["ShowResponderNotifications"] = value;
            }
        }
    }
}
