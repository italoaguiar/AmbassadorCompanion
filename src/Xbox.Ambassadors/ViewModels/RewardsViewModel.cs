using Microsoft.Xbox.Ambassadors.API.DataModel.Rewards;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.ViewModels
{
    public class RewardsViewModel : ViewModelBase
    {
        public RewardsViewModel(FrameworkElement parent) : base(parent)
        {
            RetryCommand = new CommandAdapter((p) => LoadRewards());

            LoadRewards();
        }

        bool isLoading;
        bool loadError;
        bool hasNoClaimed;
        bool hasNoUnClaimed;
        IEnumerable<Reward> claimed;
        IEnumerable<Reward> unClaimed;

        public CommandAdapter RetryCommand { get; set; }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }
        public bool LoadError
        {
            get => loadError;
            set
            {
                loadError = value;
                RaisePropertyChanged(nameof(LoadError));
            }
        }
        public IEnumerable<Reward> Claimed
        {
            get => claimed;
            set
            {
                claimed = value;
                RaisePropertyChanged(nameof(Claimed));
            }
        }
        public IEnumerable<Reward> UnClaimed
        {
            get => unClaimed;
            set
            {
                unClaimed = value;
                RaisePropertyChanged(nameof(UnClaimed));
            }
        }

        public bool HasNoClaimed
        {
            get => hasNoClaimed;
            set
            {
                hasNoClaimed = value;
                RaisePropertyChanged(nameof(HasNoClaimed));
            }
        }

        public bool HasNoUnClaimed
        {
            get => hasNoUnClaimed;
            set
            {
                hasNoUnClaimed = value;
                RaisePropertyChanged(nameof(HasNoUnClaimed));
            }
        }

        private async void LoadRewards()
        {
            try
            {
                IsLoading = true;
                LoadError = false;

                var token = await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false);

                var rs = await Microsoft.Xbox.Ambassadors.API.Rewards.GetRewardedSessionsAsync(token).ConfigureAwait(true);

                UnClaimed = await Microsoft.Xbox.Ambassadors.API.Rewards.GetRewardsAsync(token, rs.LastOrDefault()).ConfigureAwait(true);

                //Claimed = await Microsoft.Xbox.Ambassadors.API.Rewards.GetRewardsAsync(await AccessToken.LoadFromVaultOrGetNew(), currentSeason.SeasonId);

                Claimed = UnClaimed;

                HasNoClaimed = Claimed == null || Claimed.Count() == 0;
                HasNoUnClaimed = UnClaimed == null || UnClaimed.Count() == 0;
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
            LoadRewards();
        }
    }
}
