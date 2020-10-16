using System.Collections.Generic;
using Windows.Storage;

namespace Xbox.Ambassadors.BackgroundTasks
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

        public static IList<int> LastPostsNotified
        {
            get
            {
                var r = localSettings.Values["LastPostsNotified"];

                return r == null ? null : System.Text.Json.JsonSerializer.Deserialize<IList<int>>(r.ToString());
            }
            set
            {
                localSettings.Values["LastPostsNotified"] = System.Text.Json.JsonSerializer.Serialize(value);
            }
        }
    }
}
