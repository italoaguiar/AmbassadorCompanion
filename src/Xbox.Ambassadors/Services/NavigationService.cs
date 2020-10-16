using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Xbox.Ambassadors.Services
{
    public class NavigationService
    {
        private static Frame contentFrame;

        public Frame ContentFrame { get => contentFrame; set => contentFrame = value; }

        public static void SetContentFrame(Frame f)
        {
            contentFrame = f;
        }

        public static void AddRoute(string route, Type page)
        {
            if (!Routes.ContainsKey(route))
                Routes.Add(route, page);
        }

        private static Dictionary<string, Type> Routes = new Dictionary<string, Type>();

        public void Navigate(string route, object parameter)
        {
            if (contentFrame != null)
            {
                contentFrame.Navigate(Routes[route], parameter);
            }
        }

        public void Navigate(string route)
        {
            Navigate(route, null);
        }
    }
}
