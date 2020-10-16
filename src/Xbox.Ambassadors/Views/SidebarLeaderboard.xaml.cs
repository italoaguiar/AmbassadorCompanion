using Windows.UI.Xaml.Controls;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SidebarLeaderboard : Page, INavigableTo
    {
        public SidebarLeaderboard()
        {
            this.InitializeComponent();
            ViewModel = new LeaderboardViewModel(this);
        }

        public LeaderboardViewModel ViewModel { get; set; }

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
