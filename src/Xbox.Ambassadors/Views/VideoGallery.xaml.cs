using GoogleAnalytics;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Controls;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class VideoGallery : Page, INavigableTo
    {
        public VideoGallery()
        {
            this.InitializeComponent();
            ViewModel = new VideoGalleryViewModel(this);
        }

        public VideoGalleryViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Video Gallery").Build());
            }
            catch { }
        }

        private void LikeBtn_RequestForLike(object sender, Controls.LikeEventArgs e)
        {

        }

        private void DislikeBtn_RequestForDislike(object sender, Controls.DislikeEventArgs e)
        {

        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            VideoDialog vd = new VideoDialog();
            vd.ViewModel = ViewModel;
            ViewModel.SelectedItem = e.ClickedItem as GalleryItem;
            vd.LoadVideo(ViewModel.SelectedItem.Id);
            ViewModel.IsGalleryControlsEnabled = false;
            await vd.ShowAsync();
        }
        private async void ListView2_ItemClick(object sender, ItemClickEventArgs e)
        {
            VideoDialog vd = new VideoDialog();
            vd.ViewModel = ViewModel;
            vd.IsSubmissionForm = true;
            ViewModel.SelectedItem = e.ClickedItem as Microsoft.Xbox.Ambassadors.API.YouTube.SearchResponseModels.Item;
            vd.LoadVideo(ViewModel.SelectedItem.Id);
            ViewModel.SubmitVideoCommand.CanExecuteAction = false;
            await vd.ShowAsync();
        }

        private void Web_ScriptNotify(object sender, NotifyEventArgs e)
        {
            ViewModel.IsGalleryControlsEnabled = true;
        }
        private void Web2_ScriptNotify(object sender, NotifyEventArgs e)
        {
            ViewModel.SubmitVideoCommand.CanExecuteAction = true;
        }



        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.Search(args.QueryText);
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = await VideoGalleryViewModel.FindSugestions(sender.Text);
            }
        }


    }
}
