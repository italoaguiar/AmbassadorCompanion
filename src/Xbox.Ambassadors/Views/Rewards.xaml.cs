using GoogleAnalytics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Rewards : Page, INavigableTo
    {
        public Rewards()
        {
            this.InitializeComponent();
            ViewModel = new RewardsViewModel(this);
        }

        public RewardsViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Rewards").Build());
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
