﻿using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class SidebarXboxLiveStatus : Page, INavigableTo
    {
        public SidebarXboxLiveStatus()
        {
            this.InitializeComponent();
            ViewModel = new XboxLiveStatusViewModel(this);
        }

        public XboxLiveStatusViewModel ViewModel { get; set; }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }

        private async void RefreshContainer_RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            var deferral = args.GetDeferral();

            ViewModel.UpdateViewModel();

            await Task.Delay(1000);

            deferral.Complete();
        }
    }
}
