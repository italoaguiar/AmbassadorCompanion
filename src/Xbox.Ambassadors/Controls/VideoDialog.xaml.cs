using Microsoft.Graphics.Canvas.Text;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xbox.Ambassadors.ViewModels;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Controls
{
    public sealed partial class VideoDialog : ContentDialog
    {
        public VideoDialog()
        {
            this.InitializeComponent();
            this.Closing += VideoDialog_Closing;

            if (Gamepad.Gamepads.Count > 0)
                HandleGamepad(Gamepad.Gamepads[0]);
        }

        private async void HandleGamepad(Gamepad g)
        {
            while (this.IsLoaded)
            {
                if (g.GetCurrentReading().Buttons == GamepadButtons.B)
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                        Hide();
                        return;
                        });
            }
        }


        private void VideoDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            web.NavigateToString("");
        }

        private void LikeBtn_RequestForLike(object sender, Controls.LikeEventArgs e)
        {

        }

        private void DislikeBtn_RequestForDislike(object sender, Controls.DislikeEventArgs e)
        {

        }

        private void Web_ScriptNotify(object sender, NotifyEventArgs e)
        {
            ViewModel.IsGalleryControlsEnabled = true;
            ViewModel.SubmitVideoCommand.CanExecuteAction = true;
        }


        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(VideoGalleryViewModel), typeof(VideoDialog),
                new PropertyMetadata(null));


        public VideoGalleryViewModel ViewModel
        {
            get => (VideoGalleryViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static readonly DependencyProperty IsSubmissionFormProperty =
            DependencyProperty.Register("IsSubmissionForm", typeof(bool), typeof(VideoDialog),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIsSubmissionFormChanged)));

        private static void OnIsSubmissionFormChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoDialog vd = d as VideoDialog;
            bool isIsSubmissionForm = (bool)e.NewValue;

            vd.ratingControls.Visibility = isIsSubmissionForm ? Visibility.Collapsed : Visibility.Visible;
            vd.submitButton.Visibility = isIsSubmissionForm ? Visibility.Visible : Visibility.Collapsed;
            vd.statisticsBox.Visibility = isIsSubmissionForm ? Visibility.Collapsed : Visibility.Visible;
        }

        public bool IsSubmissionForm
        {
            get => (bool)GetValue(IsSubmissionFormProperty);
            set => SetValue(IsSubmissionFormProperty, value);
        }

        string htmlBody;


        private async void InjectHTML(string videoId, WebView target)
        {
            if (htmlBody == null)
            {
                var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Html/GalleryEmbedVideo.html"));
                Stream fileStream = await storageFile.OpenStreamForReadAsync();

                using (StreamReader sr = new StreamReader(fileStream))
                {
                    htmlBody = await sr.ReadToEndAsync();
                }
            }
            var body = htmlBody.Replace("{0}", videoId);
            target.NavigateToString(body);
        }

        private async void DownvoteButtonClick(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1000);
            fly.Hide();
        }

        public void LoadVideo(string videoId)
        {
            InjectHTML(videoId, web);
        }



        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
