using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API;
using Microsoft.Xbox.Ambassadors.API.DataModel.Specializations;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.ViewModels.Profile
{
    public class ProfileSpecializationsViewModel : ViewModelBase
    {
        public ProfileSpecializationsViewModel(FrameworkElement parent) : base(parent)
        {

        }

        Specialization[] specializations;
        bool isEnabled;

        public Specialization[] UserSpecializations
        {
            get => specializations;
            set
            {
                specializations = value;
                RaisePropertyChanged("UserSpecializations");
            }
        }

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                RaisePropertyChanged("IsEnabled");

                if (!value)
                {
                    UpdateSpecializations();
                }
            }
        }

        private async void LoadSpecializations()
        {
            try
            {
                UserSpecializations = await Specializations.GetAsync(await AccessToken.LoadFromVaultOrGetNew());
            }
            catch
            {

            }
        }

        private async void UpdateSpecializations()
        {
            try
            {
                await Specializations.PutAsync(await AccessToken.LoadFromVaultOrGetNew(), UserSpecializations);

                App.AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("Specialization", "Update").Build());
            }
            catch
            {

            }
        }
        public override void UpdateViewModel()
        {
            LoadSpecializations();
        }
    }
}
