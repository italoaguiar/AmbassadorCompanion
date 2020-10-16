using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;
using Xbox.Ambassadors.ViewModels.Profile;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views.ProfileViews
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ProfileOverview : Page, INavigableTo
    {
        public ProfileOverview()
        {
            this.InitializeComponent();
            ViewModel = new ProfileOverviewViewModel(this);
        }

        public ProfileOverviewViewModel ViewModel { get; set; }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        public ImageSource FromUri(Uri url)
        {
            BitmapImage bmp = new BitmapImage(url);
            return bmp;
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }
    }
}
