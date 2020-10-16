using Microsoft.Xbox.Ambassadors.API;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;
using Xbox.Ambassadors.ViewModels.Profile;

// The Blank Page item template is documented in https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views.ProfileViews
{
    /// <summary>
    /// An empty page that can be used standalone or in navigation within a Frame.
    /// </summary>
    public sealed partial class ProfileActivities : Page, INavigableTo
    {
        public ProfileActivities()
        {
            this.InitializeComponent();
            ViewModel = new ProfileActivitiesViewModel(this);
        }
        public ProfileActivitiesViewModel ViewModel { get; set; }



        public static readonly DependencyProperty ActivityTypeProperty =
            DependencyProperty.Register("ActivityType", typeof(ActivityType), typeof(ProfileActivities), new PropertyMetadata(ActivityType.activities, OnActivityTypeChanged));
        private static void OnActivityTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var k = (ProfileActivities)d;
            k.ViewModel.ActivityType = (ActivityType)e.NewValue;
        }
        public ActivityType ActivityType
        {
            get
            {
                return (ActivityType)this.GetValue(ActivityTypeProperty);
            }
            set
            {

                this.SetValue(ActivityTypeProperty, value);
            }
        }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }
    }
}
