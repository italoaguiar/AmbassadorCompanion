using GoogleAnalytics;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Auth;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Settings : Page, INavigableTo
    {
        public Settings()
        {
            this.InitializeComponent();
            ViewModel = new SettingsViewModel(this);

            this.Loaded += Settings_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Settings").Build());
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public SettingsViewModel ViewModel { get; set; }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }
        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        private async void SignOut_Click(object sender, RoutedEventArgs e)
        {

            AccessToken.ClearVault();
            await WebView.ClearTemporaryWebDataAsync();

            AppRestartFailureReason result =
            await CoreApplication.RequestRestartAsync("");
            if (result == AppRestartFailureReason.NotInForeground ||
                result == AppRestartFailureReason.RestartPending ||
                result == AppRestartFailureReason.Other)
            {
                Application.Current.Exit();
            }

        }

        private void ResponderNotificationsChanged(object sender, RoutedEventArgs e)
        {
            AppSettings.ShowResponderNotifications = (sender as ToggleSwitch).IsOn;
        }
    }
}
