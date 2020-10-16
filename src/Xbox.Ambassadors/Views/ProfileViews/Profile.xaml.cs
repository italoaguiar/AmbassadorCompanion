using GoogleAnalytics;
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
    public sealed partial class Profile : Page, INavigableTo
    {
        public Profile()
        {
            this.InitializeComponent();
            ViewModel = new ProfileViewModel(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Profile").Build());
        }

        public ProfileViewModel ViewModel { get; set; }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
            ((INavigableTo)((PivotItem)pv.SelectedItem).Content).RefreshModel();
        }

        private void Pivot_PivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            if (!((Page)args.Item.Content).IsEnabled)
            {
                ((INavigableTo)args.Item.Content).RefreshModel();
                ((Page)args.Item.Content).IsEnabled = true;
            }
        }
    }
}
