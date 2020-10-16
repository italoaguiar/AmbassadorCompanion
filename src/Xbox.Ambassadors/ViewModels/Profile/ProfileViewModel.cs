using Microsoft.Xbox.Ambassadors.API;
using Windows.UI.Xaml;

namespace Xbox.Ambassadors.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public ProfileViewModel(FrameworkElement parent) : base(parent)
        {

        }

        public Profiles Profile
        {
            get
            {
                return AmbassadorProfile.Profile;
            }
        }
        public Identity Identity
        {
            get
            {
                return AmbassadorProfile.UserIdentity;
            }
        }

        public override void UpdateViewModel()
        {

        }
    }
}
