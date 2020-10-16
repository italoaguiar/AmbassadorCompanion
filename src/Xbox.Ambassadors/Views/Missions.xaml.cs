using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API.DataModel.Missions;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Missions : Page, INavigableTo
    {
        public Missions()
        {
            this.InitializeComponent();
            ViewModel = new MissionsViewModel(this);
        }

        public MissionsViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Missions").Build());
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //ViewModel.NavigationService.Navigate("MissionDetails", e.ClickedItem);
            MissionDetailsDialog mdd = new MissionDetailsDialog();
            mdd.Mission = e.ClickedItem as Mission;
            await mdd.ShowAsync();
        }
        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }
    }
}
