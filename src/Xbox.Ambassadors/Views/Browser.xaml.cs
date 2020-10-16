using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Browser : Page, INavigableTo
    {
        public Browser()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var p = e.Parameter as Uri;
            if (p != null)
            {
                web.Navigate(p);
            }
        }

        public ViewModelBase GetViewModel()
        {
            return null;
        }

        public void RefreshModel()
        {
            web.Refresh();
        }
    }
}
