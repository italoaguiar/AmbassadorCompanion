using Microsoft.Xbox.Ambassadors.API;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;
using Xbox.Ambassadors.IncrementalLoading;

namespace Xbox.Ambassadors.ViewModels
{
    public class LeaderboardViewModel : ViewModelBase
    {
        public LeaderboardViewModel(FrameworkElement parent) : base(parent)
        {
            RetryCommand = new CommandAdapter((p) => LoadLeaderboard());
            LoadLeaderboard();
        }

        bool isLoading;
        bool loadError;
        LeaderboardLoader leaderboard;
        Item myRank;


        public CommandAdapter RetryCommand { get; set; }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }
        public bool LoadError
        {
            get => loadError;
            set
            {
                loadError = value;
                RaisePropertyChanged("LoadError");
            }
        }

        public LeaderboardLoader Leaderboard
        {
            get => leaderboard;
            set
            {
                leaderboard = value;
                RaisePropertyChanged("Leaderboard");
            }
        }

        public Item MyRank
        {
            get => myRank;
            set
            {
                myRank = value;
                RaisePropertyChanged("MyRank");
            }
        }

        private async void LoadLeaderboard()
        {
            try
            {
                IsLoading = true;
                LoadError = false;

                Leaderboard = new LeaderboardLoader((uint)AmbassadorProfile.SeasonId);

                MyRank = await Microsoft.Xbox.Ambassadors.API.Leaderboard.GetAsync(await AccessToken.LoadFromVaultOrGetNew(), (uint)AmbassadorProfile.SeasonId, AmbassadorProfile.Profile.Gamertag);
            }
            catch
            {
                LoadError = true;
            }
            finally
            {
                IsLoading = false;
            }

        }
        public override void UpdateViewModel()
        {
            LoadLeaderboard();

        }
    }
}
