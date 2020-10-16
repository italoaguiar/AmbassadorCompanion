using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API.DataModel.Missions;
using System.ComponentModel;
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
    public sealed partial class MissionDetails : Page, INotifyPropertyChanged, INavigableTo
    {
        public MissionDetails()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Mission = e.Parameter as Mission;

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Mission Details").Build());
        }

        public void RefreshModel()
        {
            //nop
        }

        private Mission mission;
        public Mission Mission
        {
            get => mission;
            set
            {
                mission = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Mission"));
            }
        }
        public ViewModelBase GetViewModel()
        {
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
