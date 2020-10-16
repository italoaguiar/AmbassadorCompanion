using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
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
    public sealed partial class Handbook : Page, INavigableTo
    {
        public Handbook()
        {
            this.InitializeComponent();
            ViewModel = new HandbookViewModel(this);
        }

        public HandbookViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Handbook").Build());
        }


        private async void OnListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv.SelectedItem != null)
            {
                HandbookSection hs = lv.SelectedItem as HandbookSection;
                string html = await ViewModel.ToHtml(hs.SectionContent);
                wv.Height = 400;
                await Task.Delay(10);
                wv.NavigateToString(html);
                sv.ChangeView(0, 0, null);
            }
        }

        private async void Wv_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var r = await wv.InvokeScriptAsync("eval", new string[] { "getDocHeight();" });
            int h = 600;

            int.TryParse(r, out h);

            wv.Height = h + 20;

        }


        private void RequestRetry_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateViewModel();
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }
    }
}
