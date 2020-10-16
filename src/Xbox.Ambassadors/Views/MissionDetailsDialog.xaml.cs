using Microsoft.Xbox.Ambassadors.API.DataModel.Missions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Views
{
    public sealed partial class MissionDetailsDialog : ContentDialog
    {
        public MissionDetailsDialog()
        {
            this.InitializeComponent();

            bool isHide = true;

            Window.Current.CoreWindow.PointerPressed += (s, e) =>
            {
                if(isHide)
                    Hide();
            };

            PointerExited += (s, e) => isHide = true;
            PointerEntered += (s, e) => isHide = false;
        }



        private Mission mission;
        public Mission Mission
        {
            get => mission;
            set
            {
                mission = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Mission"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
