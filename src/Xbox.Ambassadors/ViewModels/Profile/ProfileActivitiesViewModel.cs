using Microsoft.Xbox.Ambassadors.API;
using Microsoft.Xbox.Ambassadors.API.DataModel.Activities;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.ViewModels.Profile
{
    public class ProfileActivitiesViewModel : ViewModelBase
    {
        public ProfileActivitiesViewModel(FrameworkElement parent) : base(parent)
        {

        }

        DailyActivities[] activities;

        public ActivityType ActivityType { get; set; } = ActivityType.activities;

        public DailyActivities[] UserActivities
        {
            get => activities;
            set
            {
                activities = value;
                RaisePropertyChanged("UserActivities");
            }
        }

        private async void LoadActivities()
        {
            try
            {
                int i = 1;
                List<DailyActivities> act = new List<DailyActivities>();
                while (true)
                {
                    var r = await Activities.GetAsync(await AccessToken.LoadFromVaultOrGetNew(), ActivityType, i);
                    i++;

                    if (r == null || i == 5 || r.Length == 0)
                        break;

                    act.AddRange(r);
                }
                UserActivities = act.ToArray();
            }
            catch
            {

            }
        }

        public override void UpdateViewModel()
        {
            LoadActivities();
        }
    }
}
