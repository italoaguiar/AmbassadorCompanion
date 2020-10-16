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
    public sealed partial class Leaderboard : Page, INavigableTo
    {
        public Leaderboard()
        {
            this.InitializeComponent();
            ViewModel = new LeaderboardViewModel(this);
        }

        public LeaderboardViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Leaderboard").Build());
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
