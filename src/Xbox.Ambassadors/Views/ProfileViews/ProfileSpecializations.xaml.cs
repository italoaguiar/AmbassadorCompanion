﻿using Windows.UI.Xaml.Controls;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;
using Xbox.Ambassadors.ViewModels.Profile;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views.ProfileViews
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ProfileSpecializations : Page, INavigableTo
    {
        public ProfileSpecializations()
        {
            this.InitializeComponent();
            ViewModel = new ProfileSpecializationsViewModel(this);
        }

        public ProfileSpecializationsViewModel ViewModel { get; set; }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }

        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }
    }
}
