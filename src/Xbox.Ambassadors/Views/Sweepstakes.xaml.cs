using GoogleAnalytics;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Controls;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sweepstakes : Page, INavigableTo
    {
        public Sweepstakes()
        {
            this.InitializeComponent();
            ViewModel = new SweepstakesViewModel(this);

            l1.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        public SweepstakesViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Sweepstakes").Build());
        }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }

        private async void OnItemClicked(object sender, ItemClickEventArgs e)
        {
            SubmitTicketsDialog std = new SubmitTicketsDialog();
            std.Maximum = AmbassadorProfile.Profile.AvailableSweepstakeTickets;

            ContentDialogResult r = await std.ShowAsync();

            if (r == ContentDialogResult.Primary)
            {
                Microsoft.Xbox.Ambassadors.API.Sweepstakes s = e.ClickedItem as Microsoft.Xbox.Ambassadors.API.Sweepstakes;
                var tk = await ViewModel.SubmitSweepstakes(std.Value, s.SweepstakeDefinitionId);
                if (tk)
                {
                    s.TicketsEntered = s.TicketsEntered != null ? s.TicketsEntered + std.Value : std.Value;
                    AmbassadorProfile.Profile.AvailableSweepstakeTickets -= std.Value;
                }
            }

        }
    }
}
