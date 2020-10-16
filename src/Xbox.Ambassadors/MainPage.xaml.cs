using System;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;
using Xbox.Ambassadors.Views;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace Xbox.Ambassadors
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new MainViewModel(this);



            Window.Current.SetTitleBar(titleBar);

            var nav = SystemNavigationManager.GetForCurrentView();
            nav.BackRequested += App_BackRequested;





            NavigationService.SetContentFrame(contentFrame);
            NavigationService.AddRoute("Home", typeof(Home));
            NavigationService.AddRoute("Missions", typeof(Missions));
            NavigationService.AddRoute("MissionDetails", typeof(MissionDetails));
            NavigationService.AddRoute("Forum", typeof(Forum));
            NavigationService.AddRoute("Handbook", typeof(Handbook));
            NavigationService.AddRoute("Academy", typeof(Academy));
            NavigationService.AddRoute("Quizz", typeof(Quizz));
            NavigationService.AddRoute("VideoGallery", typeof(VideoGallery));
            NavigationService.AddRoute("Profile", typeof(Profile));
            NavigationService.AddRoute("Sweepstakes", typeof(Sweepstakes));
            NavigationService.AddRoute("Rewards", typeof(Rewards));
            NavigationService.AddRoute("Browser", typeof(Browser));
            NavigationService.AddRoute("Notifications", typeof(Notifications));
            NavigationService.AddRoute("XboxLiveStatus", typeof(XboxLiveStatus));
            NavigationService.AddRoute("Leaderboard", typeof(Leaderboard));
            NavigationService.AddRoute("Toolbox", typeof(Toolbox));
            NavigationService.AddRoute("Settings", typeof(Settings));


            //update current view for any events 
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 30);

            dt.Tick += (s, a) => UpdateCurrentView();
            NetworkInformation.NetworkStatusChanged += (s) => UpdateCurrentView();
            Application.Current.Resuming += (s, a) => UpdateCurrentView();
            //dt.Start();


            if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                this.feedbackBtn.Visibility = Visibility.Visible;
            }

            this.Loaded += MainPage_Loaded;

  
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

        }

        private async void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            //refresh the view
            if (args.VirtualKey == VirtualKey.GamepadY)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    INavigableTo page = contentFrame.Content as INavigableTo;
                    page.RefreshModel();
                });
            }

            //show notifications
            if (args.VirtualKey == VirtualKey.GamepadX)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewModel.NavigationService.Navigate("Notifications");
                });
            }
        }


        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigationService.Navigate("Home");

            App.RegisterBackgroundTask();
        }

        public MainViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (contentFrame.CanGoBack)
            {
                contentFrame.GoBack();
                e.Handled = true;
            }
            e.Handled = true;
        }



        private void UpdateCurrentView()
        {
            try
            {
                (contentFrame.Content as INavigableTo)?.GetViewModel()?.Tick();
                ViewModel.Tick();
            }
            catch { }
        }


        private void Nv_Loaded(object sender, RoutedEventArgs e)
        {
            nv.IsPaneOpen = false;

        }


        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            switch (e.SourcePageType)
            {
                case Type c when e.SourcePageType == typeof(Home):
                    ((NavigationViewItem)nv.MenuItems[0]).IsSelected = true;
                    break;
                case Type c when e.SourcePageType == typeof(Missions):
                    ((NavigationViewItem)nv.MenuItems[1]).IsSelected = true;
                    break;
                case Type c when e.SourcePageType == typeof(MissionDetails):
                    ((NavigationViewItem)nv.MenuItems[1]).IsSelected = true;
                    break;
                case Type c when e.SourcePageType == typeof(Forum):
                    ((NavigationViewItem)nv.MenuItems[2]).IsSelected = true;
                    break;
                case Type c when e.SourcePageType == typeof(Sweepstakes):
                    ((NavigationViewItem)nv.MenuItems[4]).IsSelected = true;
                    break;
                case Type c when e.SourcePageType == typeof(Rewards):
                    ((NavigationViewItem)nv.MenuItems[5]).IsSelected = true;
                    break;
                case Type c when e.SourcePageType == typeof(VideoGallery):
                    ((NavigationViewItem)nv.MenuItems[6]).IsSelected = true;
                    break;
                case Type c when e.SourcePageType == typeof(Academy):
                    ((NavigationViewItem)nv.MenuItems[7]).IsSelected = true;
                    break;
                //case Type c when e.SourcePageType == typeof(Leaderboard):
                //    ((NavigationViewItem)nv.MenuItems[8]).IsSelected = true;
                //    break;
                case Type c when e.SourcePageType == typeof(Handbook):
                    ((NavigationViewItem)nv.MenuItems[8]).IsSelected = true;
                    break;
                    //case Type c when e.SourcePageType == typeof(Help):
                    //    ((NavigationViewItem)nv.MenuItems[10]).IsSelected = true;
                    //    break;

            }
        }

        private void Nv_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            string r = (args.InvokedItem as string);

            switch (r)
            {
                case "Home":
                    ViewModel.NavigationService.Navigate("Home");
                    break;
                case "Missions":
                    ViewModel.NavigationService.Navigate("Missions");
                    break;
                case "Forum":
                    ViewModel.NavigationService.Navigate("Forum");
                    break;
                case "Sweepstakes":
                    ViewModel.NavigationService.Navigate("Sweepstakes");
                    break;
                case "Rewards":
                    ViewModel.NavigationService.Navigate("Rewards");
                    break;
                case "Academy":
                    ViewModel.NavigationService.Navigate("Academy");
                    break;
                case "Handbook":
                    ViewModel.NavigationService.Navigate("Handbook");
                    break;
                case "Video Gallery":
                    ViewModel.NavigationService.Navigate("VideoGallery");
                    break;
                case "Leaderboard":
                    ViewModel.NavigationService.Navigate("Leaderboard");
                    break;
                case "Help":
                    LaunchHelp();
                    break;

            }

            if (args.IsSettingsInvoked)
            {
                ViewModel.NavigationService.Navigate("Settings");
            }
        }

        private async void LaunchHelp()
        {
            MessageDialog md = new MessageDialog("Support for the app is available through the ambassadors forum. Do you want to proceed?");
            md.Title = "You are leaving the app";

            md.Commands.Add(new UICommand()
            {
                Label = "Go to the forum",
                Invoked = async (p) =>
                {
                    await Launcher.LaunchUriAsync(new Uri("https://answers.microsoft.com/en-us/xbambassadors/forum"));
                }
            });
            md.Commands.Add(new UICommand()
            {
                Label = "Cancel"
            });

            md.CancelCommandIndex = 1;

            await md.ShowAsync();

        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            INavigableTo page = contentFrame.Content as INavigableTo;
            page.RefreshModel();

            var side = (rightPivot.SelectedItem as PivotItem)?.Content as INavigableTo;
            side?.RefreshModel();
        }

        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            contentFrame.GoBack();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigationService.Navigate("Profile");
        }

        private void Profile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.NavigationService.Navigate("Profile");
        }

        private async void FeedbackBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private void TopbarButton_Click(object sender, RoutedEventArgs e)
        {
            var t = (sender as FrameworkElement)?.Tag?.ToString();
            if (t != null)
            {
                switch (t)
                {
                    case "Notifications":
                        ViewModel.NavigationService.Navigate("Notifications");
                        break;
                    case "Leaderboard":
                        ViewModel.NavigationService.Navigate("Leaderboard");
                        break;
                    case "LiveStatus":
                        ViewModel.NavigationService.Navigate("XboxLiveStatus");
                        break;
                    case "Toolbox":
                        ViewModel.NavigationService.Navigate("Toolbox");
                        break;
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NavigationViewItem_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.OriginalKey == VirtualKey.GamepadA)
                ViewModel.NavigationService.Navigate("Profile");
        }
    }
}
