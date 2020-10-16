using Microsoft.Xbox.Ambassadors.API;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace Xbox.Ambassadors.Controls
{
    public sealed partial class Podium : UserControl
    {
        public Podium()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PersonPictureAnimation.Begin();
        }

        public static readonly DependencyProperty LeaderboardProperty =
            DependencyProperty.Register("Leaderboard", typeof(Leaderboard), typeof(Podium), new PropertyMetadata(false, OnLeaderboardChanged));
        private static void OnLeaderboardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var k = (Podium)d;
            var l = e.NewValue as Leaderboard;
            if (l != null)
            {
                k.ConvertItems(l);
            }
        }
        public Leaderboard Leaderboard
        {
            get
            {
                return (Leaderboard)this.GetValue(LeaderboardProperty);
            }
            set
            {

                this.SetValue(LeaderboardProperty, value);
            }
        }


        private void ConvertItems(Leaderboard leaderboard)
        {
            List<CItem> l = new List<CItem>();
            int i = 1;
            foreach (var t in leaderboard.Items)
            {
                l.Add(new CItem() { Item = t, Index = i });
                i++;
            }
            lvItems.ItemsSource = l;

            person.Text = l[1].Item.Id;
            ToolTipService.SetToolTip(personPicture, l[1].Item.Id);

            person1.Text = l[0].Item.Id;
            ToolTipService.SetToolTip(personPicture1, l[0].Item.Id);

            person2.Text = l[2].Item.Id;
            ToolTipService.SetToolTip(personPicture2, l[2].Item.Id);
        }

        private class CItem
        {
            public int Index { get; set; }
            public Item Item { get; set; }
        }
    }
}
