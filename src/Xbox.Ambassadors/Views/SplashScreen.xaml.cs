using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Security.Authentication.Web.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Auth;
using Xbox.Ambassadors.Controls;
using Xbox.Ambassadors.Services;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ExtendedSplashScreen : Page
    {

        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private SplashScreen splash; // Variable to hold the splash screen object.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;


        public ExtendedSplashScreen(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This ensures that the extended splash screen formats properly in response to window resizing.
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);


            splash = splashscreen;
            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, object>(DismissedEventHandler);



                // Retrieve the window coordinates of the splash screen image.
                setSplashImageRect();
                PositionImage();
                PositionCommandsContainer();

                Loaded += ExtendedSplashScreen_Loaded;

            }


            // Create a Frame to act as the navigation context
            rootFrame = new Frame();

            this.PreviewKeyDown += ExtendedSplashScreen_PreviewKeyDown;
        }
        private static bool IsCtrlKeyPressed()
        {
            var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
            return (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
        }

        private async void ExtendedSplashScreen_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (IsCtrlKeyPressed())
            {
                if (e.Key == VirtualKey.F10)
                {
                    TokenDialog td = new TokenDialog();
                    var r = await td.ShowAsync();

                    if (r == ContentDialogResult.Primary)
                    {
                        //for testing purposes by the windows store.
                        AccessToken t = new AccessToken()
                        {
                            Token = td.Token,
                            TokenType = "Bearer",
                            ExpiresIn = 1200
                        };
                        t.SaveInValt();

                        DoLogin();
                    }
                }
            }
            if (e.Key == VirtualKey.F11)
            {
                AccessToken.ClearVault();
            }
        }

        private void setSplashImageRect()
        {
            splashImageRect = splash.ImageLocation;
            if (!Windows.UI.ViewManagement.ApplicationViewScaling.DisableLayoutScaling)
            {
                var k = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().ResolutionScale;
                double f = 1;
                switch (k)
                {
                    case Windows.Graphics.Display.ResolutionScale.Scale100Percent:
                        f = 1;
                        break;
                    case Windows.Graphics.Display.ResolutionScale.Scale200Percent:
                        f = 2;
                        break;
                    case Windows.Graphics.Display.ResolutionScale.Scale225Percent:
                        f = 2.25;
                        break;
                    case Windows.Graphics.Display.ResolutionScale.Scale250Percent:
                        f = 2.5;
                        break;
                    case Windows.Graphics.Display.ResolutionScale.Scale300Percent:
                        f = 3;
                        break;
                    case Windows.Graphics.Display.ResolutionScale.Scale350Percent:
                        f = 3.5;
                        break;
                }
                //scale for xbox 
                splashImageRect =
                    new Rect(
                        splashImageRect.Left / f,
                        splashImageRect.Top / f,
                        splashImageRect.Width / f,
                        splashImageRect.Height / f);
            }
        }

        private async void ExtendedSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(100);

            btnLogin.Focus(FocusState.Keyboard);

            DoLogin();

            btnLogin.Visibility = Visibility.Collapsed;

            //if (AccessToken.IsTokenValid)
            //{
            //    DoLogin();
            //}
            //else
            //{
            //    btnLogin.Visibility = Visibility.Visible;
            //}
        }

        void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            extendedSplashImage.Height = splashImageRect.Height;
            extendedSplashImage.Width = splashImageRect.Width;
        }

        void PositionCommandsContainer()
        {
            commandsContainer.SetValue(Canvas.LeftProperty, splashImageRect.X);
            commandsContainer.SetValue(Canvas.TopProperty, splashImageRect.Y + splashImageRect.Height);
            commandsContainer.Width = splashImageRect.Width;
        }

        private void DismissedEventHandler(SplashScreen sender, object args)
        {

        }

        private void ExtendedSplash_OnResize(object sender, WindowSizeChangedEventArgs e)
        {
            if (splash != null)
            {
                splashImageRect = splash.ImageLocation;
                PositionImage();
                PositionCommandsContainer();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }


        async void DismissExtendedSplash()
        {
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattableTitleBar.ButtonBackgroundColor = Colors.Black;
            formattableTitleBar.BackgroundColor = Colors.Black;
            formattableTitleBar.ButtonInactiveBackgroundColor = Colors.Black;
            formattableTitleBar.InactiveBackgroundColor = Colors.Black;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
                rootFrame.Navigate(typeof(MainPage));

            });
        }

        private async Task<bool> GetUserDataAsync()
        {
            prog.Value = 35;

            var r = await AmbassadorProfile.LoadAsync(Dispatcher); 

            prog.Value = 60;

            await Task.Delay(100).ConfigureAwait(true);

            prog.Value = 80;

            await Task.Delay(100).ConfigureAwait(true);

            prog.Value = 100;

            await Task.Delay(200).ConfigureAwait(true);

            if (!r)
            {
                prog.Value = 0;
                statusCotaniner.Visibility = Visibility.Collapsed;
                btnLogin.Visibility = Visibility.Visible;
                btnLogin.IsEnabled = true;

                return false;
            }

            return true;
        }

        private async void DoLogin()
        {
            btnLogin.IsEnabled = false;
            statusCotaniner.Visibility = Visibility.Visible;


            if (await GetUserDataAsync())
            {
                DismissExtendedSplash();
            }
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            DoLogin();
        }
    }
}
