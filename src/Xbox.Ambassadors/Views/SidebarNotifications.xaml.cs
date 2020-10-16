using Microsoft.Xbox.Ambassadors.API;
using Windows.UI.Xaml.Controls;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class SidebarNotifications : Page, INavigableTo
    {
        public SidebarNotifications()
        {
            this.InitializeComponent();
            ViewModel = new NotificationsViewModel(this);
        }

        public NotificationsViewModel ViewModel { get; set; }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }

        private void lv_ContextRequested(Windows.UI.Xaml.UIElement sender, Windows.UI.Xaml.Input.ContextRequestedEventArgs args)
        {
            var source = args.OriginalSource as ListViewItem;
            if (source != null)
            {
                if (source.ContextFlyout == null)
                {
                    MenuFlyout f = new MenuFlyout();
                    f.Items.Add(new MenuFlyoutItem
                    {
                        Text = "Delete",
                        Icon = new SymbolIcon(Symbol.Delete),
                        Command = ViewModel.RemoveCommand,
                        CommandParameter = (source.DataContext as Notification)?.Id
                    });
                    source.ContextFlyout = f;
                    f.ShowAt(source);
                }
            }
        }
    }
}
