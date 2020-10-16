using GoogleAnalytics;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using Microsoft.Xbox.Ambassadors.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.ViewModels
{
    public class SweepstakesViewModel : ViewModelBase
    {
        public SweepstakesViewModel(FrameworkElement parent) : base(parent)
        {
            ReloadSweepstakesCommand = new CommandAdapter((p) => LoadSweepstakes());
            ReloadUpcomingCommand = new CommandAdapter((p) => LoadUpcoming());
            ReloadExpiredCommand = new CommandAdapter((p) => LoadExpired());

            LoadSweepstakes();
            LoadUpcoming();
            LoadExpired();
        }

        public Profiles Profile
        {
            get
            {
                return AmbassadorProfile.Profile;
            }
        }

        #region Properties

        public CommandAdapter ReloadSweepstakesCommand { get; set; }
        public CommandAdapter ReloadExpiredCommand { get; set; }
        public CommandAdapter ReloadUpcomingCommand { get; set; }

        bool isLoadingSweepstakes;
        bool loadSweepstakesFailed;
        bool isLoadingExpired;
        bool loadExpiredFailed;
        bool isLoadingUpcoming;
        bool loadUpcomingFailed;
        bool hasNoUpcoming;
        bool hasNoExpired;
        bool hasNoSweepstakes;
        Sweepstakes[] sweepstakes;
        Sweepstakes[] upcoming;
        Sweepstakes[] expired;

        public bool IsLoadingSweepstakes
        {
            get => isLoadingSweepstakes;
            set
            {
                isLoadingSweepstakes = value;
                RaisePropertyChanged(nameof(IsLoadingSweepstakes));
            }
        }
        public bool LoadSweepstakesFailed
        {
            get => loadSweepstakesFailed;
            set
            {
                loadSweepstakesFailed = value;
                RaisePropertyChanged(nameof(LoadSweepstakesFailed));
            }
        }
        public Sweepstakes[] Sweepstakes
        {
            get => sweepstakes;
            set
            {
                sweepstakes = value;
                RaisePropertyChanged(nameof(Sweepstakes));
            }
        }
        public Sweepstakes[] Upcomming
        {
            get => upcoming;
            set
            {
                upcoming = value;
                RaisePropertyChanged(nameof(Upcomming));
            }
        }
        public Sweepstakes[] Expired
        {
            get => expired;
            set
            {
                expired = value;
                RaisePropertyChanged(nameof(Expired));
            }
        }

        public bool IsLoadingExpired
        {
            get => isLoadingExpired;
            set
            {
                isLoadingExpired = value;
                RaisePropertyChanged(nameof(IsLoadingExpired));
            }
        }
        public bool LoadExpiredFailed
        {
            get => loadExpiredFailed;
            set
            {
                loadExpiredFailed = value;
                RaisePropertyChanged(nameof(LoadExpiredFailed));
            }
        }
        public bool IsLoadingUpcoming
        {
            get => isLoadingUpcoming;
            set
            {
                isLoadingUpcoming = value;
                RaisePropertyChanged(nameof(IsLoadingUpcoming));
            }
        }
        public bool LoadUpcomingFailed
        {
            get => loadUpcomingFailed;
            set
            {
                loadUpcomingFailed = value;
                RaisePropertyChanged(nameof(LoadUpcomingFailed));
            }
        }

        public bool HasNoUpcoming
        {
            get => hasNoUpcoming;
            set
            {
                hasNoUpcoming = value;
                RaisePropertyChanged(nameof(HasNoUpcoming));
            }
        }
        public bool HasNoExpired
        {
            get => hasNoExpired;
            set
            {
                hasNoExpired = value;
                RaisePropertyChanged(nameof(HasNoExpired));
            }
        }
        public bool HasNoSweepstakes
        {
            get => hasNoSweepstakes;
            set
            {
                hasNoSweepstakes = value;
                RaisePropertyChanged(nameof(HasNoSweepstakes));
            }
        }


        #endregion


        private async void LoadSweepstakes()
        {
            try
            {
                IsLoadingSweepstakes = true;
                LoadSweepstakesFailed = false;

                var token = await AccessToken.LoadFromVaultOrGetNew();

                Sweepstakes = await Microsoft.Xbox.Ambassadors.API.Sweepstakes.GetAsync(token, false);

                HasNoSweepstakes = Sweepstakes == null || Sweepstakes.Length == 0;

                IsLoadingSweepstakes = false;
            }
            catch
            {
                IsLoadingSweepstakes = false;
                LoadSweepstakesFailed = true;
            }
        }


        private async void LoadUpcoming()
        {
            try
            {
                IsLoadingUpcoming = true;
                LoadUpcomingFailed = false;

                var token = await AccessToken.LoadFromVaultOrGetNew();

                Upcomming = await Microsoft.Xbox.Ambassadors.API.Sweepstakes.GetAsync(token, true);

                HasNoUpcoming = Upcomming == null || Upcomming.Length == 0;

                IsLoadingUpcoming = false;
            }
            catch
            {
                IsLoadingUpcoming = false;
                LoadUpcomingFailed = true;
            }
        }

        private async void LoadExpired()
        {
            try
            {
                IsLoadingExpired = true;
                LoadExpiredFailed = false;

                var token = await AccessToken.LoadFromVaultOrGetNew();

                var s = await Season.GetCurrentAsync(token);

                Expired = await Microsoft.Xbox.Ambassadors.API.Sweepstakes.GetExpiredAsync(token, s.SeasonId);

                HasNoExpired = Expired == null || Expired.Length == 0;

                IsLoadingExpired = false;
            }
            catch
            {
                IsLoadingExpired = false;
                LoadExpiredFailed = true;
            }
        }

        public async Task<bool> SubmitSweepstakes(long tickets, long sweepstakeId)
        {
            try
            {
                var token = await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false);
                
                var r = await Microsoft.Xbox.Ambassadors.API.Sweepstakes.PostAsync(token, tickets, sweepstakeId).ConfigureAwait(false);

                App.AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("Click", "Sweepstakes", "Submit", tickets).Build());

                return (r == "success");
            }
            catch
            {
                Services.NotificationService.Service.Notify("Error", "Failed");

                return false;
            }

        }

        public override void UpdateViewModel()
        {
            LoadSweepstakes();
            LoadUpcoming();
            LoadExpired();
        }
    }
}
