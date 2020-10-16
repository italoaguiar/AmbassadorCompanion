using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API.Forum;
using System;
using System.Linq;
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
    public sealed partial class Forum : Page, INavigableTo
    {
        public Forum()
        {
            this.InitializeComponent();
            ViewModel = new ForumViewModel(this);
        }

        public ForumViewModel ViewModel { get; set; }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                var SelectedThread = e.Parameter as ForumThread;
                ViewModel.SelectedThread = SelectedThread;
                NavigateToThread(SelectedThread);
            }

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Forum").Build());
        }

        private void FView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            pg.IsIndeterminate = true;
            string[] ep =
            {
                "/en-us/xbox/forum/all",
                "/en-us/site/startsignin",
                "/login.srf",
                "/signup",
                "/ppsecure/post.srf",
                "/en-us/site/completesignin"
            };
            if (!ep.Any(x => args.Uri.AbsolutePath.StartsWith(x)))
                args.Cancel = true;

            if (MasterListView.SelectedItem != null)
            {
                MasterListView.ScrollIntoView(MasterListView.SelectedItem);
            }
        }

        private void ThreadList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var t = e.ClickedItem as ForumThread;
            NavigateToThread(t);
        }



        private void NavigateToThread(ForumThread f)
        {
            fView.Navigate(f.ThreadUri);
        }

        private async void InjectCss()
        {
            string f = "var link = document.createElement('link'); link.rel = 'stylesheet';" +
                "link.type = 'text/css';  link.href = 'ms-appx-web:///Html/Forum.css';" +
                "document.getElementsByTagName('head')[0].appendChild(link); ";

            await fView.InvokeScriptAsync("eval", new string[] { f });
        }

        private void FView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            pg.IsIndeterminate = false;
        }

        private void FView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            InjectCss();
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
