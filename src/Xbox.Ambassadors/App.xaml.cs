using GoogleAnalytics;
using Microsoft.QueryStringDotNET;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.BackgroundTasks;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.Views;

namespace Xbox.Ambassadors
{
    /// <summary>
    ///Fornece o comportamento específico do aplicativo para complementar a classe Application padrão.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Inicializa o objeto singleton do aplicativo.  Esta é a primeira linha de código criado
        /// executado e, como tal, é o equivalente lógico de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            //bool result = ApplicationViewScaling.TrySetDisableLayoutScaling(true);
            Application.Current.RequiresPointerMode = ApplicationRequiresPointerMode.WhenRequested;
            this.RequiresPointerMode = ApplicationRequiresPointerMode.WhenRequested;
            this.Suspending += OnSuspending;
            this.UnhandledException += App_UnhandledException;

            AnalyticsManager.Current.DispatchPeriod = TimeSpan.Zero; //immediate mode, sends hits immediately
            AnalyticsManager.Current.ReportUncaughtExceptions = true; //catch unhandled exceptions and send the details
            AnalyticsManager.Current.AutoAppLifetimeMonitoring = true; //handle suspend/resume and empty hit batched hits on suspend

            AnalyticsTracker = AnalyticsManager.Current.CreateTracker("Google Analytics Key");

            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox")
            {
                this.FocusVisualKind = FocusVisualKind.Reveal;
            }
        }

        public static Tracker AnalyticsTracker { get; set; }

        public static async void RegisterBackgroundTask()
        {
            try
            {
                if (!BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name == "Background-Worker"))
                {

                    TimeTrigger trigger = new TimeTrigger(120, false);
                    SystemCondition condition = new SystemCondition(SystemConditionType.InternetAvailable | SystemConditionType.UserPresent);

                    var requestStatus = await BackgroundExecutionManager.RequestAccessAsync();
                    if (requestStatus == BackgroundAccessStatus.DeniedBySystemPolicy || requestStatus == BackgroundAccessStatus.DeniedByUser)
                    {
                        return;
                    }

                    BackgroundTaskBuilder builder = new BackgroundTaskBuilder();

                    builder.Name = "Background-Worker";
                    builder.TaskEntryPoint = "Xbox.Ambassadors.BackgroundTasks.BackgroundUpdateTask";
                    builder.AddCondition(condition);
                    builder.SetTrigger(trigger);

                    BackgroundTaskRegistration task = builder.Register();
                }
            }
            catch
            {

            }
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
            Debug.WriteLine(e.Exception.StackTrace);
        }

        /// <summary>
        /// Chamado quando o aplicativo é iniciado normalmente pelo usuário final.  Outros pontos de entrada
        /// serão usados, por exemplo, quando o aplicativo for iniciado para abrir um arquivo específico.
        /// </summary>
        /// <param name="e">Detalhes sobre a solicitação e o processo de inicialização.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            var green = Color.FromArgb(255, 16, 124, 16);
            formattableTitleBar.ButtonBackgroundColor = green;
            formattableTitleBar.BackgroundColor = green;
            formattableTitleBar.ButtonInactiveBackgroundColor = green;
            formattableTitleBar.InactiveBackgroundColor = green;

            Frame rootFrame = Window.Current.Content as Frame;

            // Não repita a inicialização do aplicativo quando a Janela já tiver conteúdo,
            // apenas verifique se a janela está ativa
            if (rootFrame == null)
            {
                // Crie um Quadro para atuar como o contexto de navegação e navegue para a primeira página
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Carregue o estado do aplicativo suspenso anteriormente
                }

                // Coloque o quadro na Janela atual
                Window.Current.Content = rootFrame;

                if (e.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    bool loadState = (e.PreviousExecutionState == ApplicationExecutionState.Terminated);
                    ExtendedSplashScreen extendedSplash = new ExtendedSplashScreen(e.SplashScreen, loadState);
                    Window.Current.Content = extendedSplash;
                }
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {

                    // Quando a pilha de navegação não for restaurada, navegar para a primeira página,
                    // configurando a nova página passando as informações necessárias como um parâmetro
                    // parâmetro
                    //rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Verifique se a janela atual está ativa
                Window.Current.Activate();
            }

        }

        /// <summary>
        /// Chamado quando notificações são clicadas
        /// </summary>
        /// <param name="args"></param>
        protected override void OnActivated(IActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Não repita a inicialização do aplicativo quando a Janela já tiver conteúdo,
            // apenas verifique se a janela está ativa
            if (rootFrame == null)
            {
                // Crie um Quadro para atuar como o contexto de navegação e navegue para a primeira página
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Carregue o estado do aplicativo suspenso anteriormente
                }

                // Coloque o quadro na Janela atual
                Window.Current.Content = rootFrame;

                if (args.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    bool loadState = (args.PreviousExecutionState == ApplicationExecutionState.Terminated);
                    ExtendedSplashScreen extendedSplash = new ExtendedSplashScreen(args.SplashScreen, loadState);
                    Window.Current.Content = extendedSplash;
                }
            }
            

            if (args is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

                // Parse the query string (using QueryString.NET)
                QueryString q = QueryString.Parse(toastActivationArgs.Argument);

                if (!q.Contains("action")) return;

                // See what action is being requested 
                switch (q["action"])
                {
                    //case "acceptResponderRequest":
                    //    ResponderService.Service.AcceptRequest(q["conversationId"]);
                    //    //if (rootFrame.Content.GetType() == typeof(MainPage))
                    //    //{
                    //    //    MainPage mp = rootFrame.Content as MainPage;
                    //    //    mp.ViewModel.NavigationService.Navigate("Responder", q["conversationId"]);
                    //    //}
                    //    AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("Responder Notification", "Accept").Build());
                    //    break;
                    //case "launchResponder":
                    //    if (rootFrame.Content.GetType() == typeof(MainPage))
                    //    {
                    //        MainPage mp = rootFrame.Content as MainPage;
                    //        mp.ViewModel.NavigationService.Navigate("Responder", q["conversationId"]);
                    //    }
                    //    AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("Responder Notification", "Launch").Build());
                    //    break;
                }
            }
        }

        /// <summary>
        /// Chamado quando ocorre uma falha na Navegação para uma determinada página
        /// </summary>
        /// <param name="sender">O Quadro com navegação com falha</param>
        /// <param name="e">Detalhes sobre a falha na navegação</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Chamado quando a execução do aplicativo está sendo suspensa.  O estado do aplicativo é salvo
        /// sem saber se o aplicativo será encerrado ou retomado com o conteúdo
        /// da memória ainda intacto.
        /// </summary>
        /// <param name="sender">A fonte da solicitação de suspensão.</param>
        /// <param name="e">Detalhes sobre a solicitação de suspensão.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Salvar o estado do aplicativo e parar qualquer atividade em segundo plano
            deferral.Complete();
        }
    }
}
