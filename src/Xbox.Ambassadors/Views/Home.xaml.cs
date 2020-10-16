using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API.DataModel.Missions;
using System;
using System.Threading;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Home : Page, INavigableTo
    {
        public Home()
        {
            this.InitializeComponent();
            ViewModel = new HomeViewModel(this);


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += Timer_Tick;
            timer.Start();

            this.Loaded += Home_Loaded;
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            //handle xbox horizontal scroll
            var forumList = FindName("forumList");
            var xbscroll = FindName("xbScroll") as ScrollViewer;
            if(forumList is ListView)
            {
                ((ListView)forumList).GotFocus += (s, a) =>
                {
                    if(xbscroll != null && xbscroll.HorizontalOffset < xbscroll.ScrollableWidth)
                        xbscroll.ChangeView(xbscroll.ScrollableWidth, null, null);
                };
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Home").Build());
        }


        public HomeViewModel ViewModel { get; set; }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (carousel.SelectedIndex < carousel.Items.Count - 1)
            {
                carousel.SelectedIndex++;
            }
            else
            {
                for (int i = carousel.Items.Count - 1; i >= 0; i--)
                {
                    carousel.SelectedIndex = i;
                    Thread.Sleep(100);
                }
            }
        }

        private ImageSource UriToImageSource(object u)
        {
            return new BitmapImage((Uri)u);
        }

        private async void Missions_Click(object sender, ItemClickEventArgs e)
        {
            //ViewModel.NavigationService.Navigate("MissionDetails", e.ClickedItem);
            MissionDetailsDialog mdd = new MissionDetailsDialog();
            mdd.Mission = e.ClickedItem as Mission;
            await mdd.ShowAsync();
        }

        private void AllMissions_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigationService.Navigate("Missions");
        }

        private void ViewAllForums_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigationService.Navigate("Forum");
        }

        private void ViewForum_Click(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigationService.Navigate("Forum", e.ClickedItem);
        }

        private void ViewProfile_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigationService.Navigate("Profile");
        }

        private async void Navigate_Click(object sender, RoutedEventArgs e)
        {
            var t = (sender as FrameworkElement).Tag.ToString();

            if (t == "/xbox/academy")
            {
                ViewModel.NavigationService.Navigate("Academy");
                return;
            }

            //ViewModel.NavigationService.Navigate("Browser", new Uri(t));

            await Launcher.LaunchUriAsync(new Uri(t));
        }
        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }


    }
}
