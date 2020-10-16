using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SidebarToolbox : Page, INavigableTo
    {
        public SidebarToolbox()
        {
            this.InitializeComponent();
            ViewModel = new ToolboxViewModel(this);
        }

        public ToolboxViewModel ViewModel { get; set; }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.Search(args.QueryText);
        }

        private void CopyLink_Click(object sender, RoutedEventArgs e)
        {
            var r = (sender as Button)?.Tag as Uri;
            if (r != null)
            {
                var dp = new DataPackage()
                {
                    RequestedOperation = DataPackageOperation.Copy
                };
                dp.SetText(r.AbsoluteUri);
                Clipboard.SetContent(dp);
            }
        }
    }
}
