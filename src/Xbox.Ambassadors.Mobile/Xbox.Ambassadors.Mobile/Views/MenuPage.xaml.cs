using Xbox.Ambassadors.Mobile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xbox.Ambassadors.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Title="Home", ImageSource = "Home.png" },
                new HomeMenuItem {Title="Missions", ImageSource = "Flag.png"  },
                new HomeMenuItem {Title="Sweepstakes", ImageSource = "Flag.png"  },
                new HomeMenuItem {Title="Rewards", ImageSource = "Gift.png"  },
                new HomeMenuItem {Title="Responder", ImageSource = "Responder.png"  },
                new HomeMenuItem {Title="Video Gallery", ImageSource = "Video.png"  },
                new HomeMenuItem {Title="Academy", ImageSource = "Bank.png"  },
                new HomeMenuItem {Title="Leaderboard", ImageSource = "Star.png"  },
                new HomeMenuItem {Title="Handbook", ImageSource = "Book.png"  },
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
           /* ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };*/
        }
    }
}