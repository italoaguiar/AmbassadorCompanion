using Microsoft.Xbox.Ambassadors.API;
using Windows.UI.Xaml;

namespace Xbox.Ambassadors.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(FrameworkElement parent) : base(parent)
        {

        }

        #region User Data
        public Profiles Profile
        {
            get { return AmbassadorProfile.Profile; }
        }

        public Identity UserIdentity
        {
            get { return AmbassadorProfile.UserIdentity; }
        }
        #endregion

        public override void UpdateViewModel()
        {

        }
    }
}
