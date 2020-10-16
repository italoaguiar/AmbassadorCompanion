using Microsoft.Xbox.Ambassadors.API;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        public NotificationsViewModel(FrameworkElement parent) : base(parent)
        {
            RegisterTimeTrigger(UpdateNotifications, new TimeSpan(0, 5, 0, 0));

            RemoveCommand = new CommandAdapter(RemoveNotification);

            UpdateNotifications();
        }

        public CommandAdapter RemoveCommand { get; set; }

        static ObservableCollection<Notification> _notifications;

        public ObservableCollection<Notification> Notifications
        {
            get => _notifications;
            set
            {
                _notifications = value;
                RaisePropertyChanged("Notifications");
            }
        }

        private async void UpdateNotifications()
        {
            try
            {
                var r = await Microsoft.Xbox.Ambassadors.API.Notifications.GetAsync(await AccessToken.LoadFromVaultOrGetNew());
                Notifications = new ObservableCollection<Notification>(r);
            }
            catch
            {

            }
        }

        private async void RemoveNotification(object param)
        {
            try
            {
                string p = param as string;
                if (p != null)
                {
                    var r = await Microsoft.Xbox.Ambassadors.API.Notifications.RemoveAsync(await AccessToken.LoadFromVaultOrGetNew(), p);
                    if (r)
                    {
                        Notifications.Remove(Notifications.First(x => x.Id == p));
                    }
                }
            }
            catch
            {

            }
        }

        public override void UpdateViewModel()
        {
            UpdateNotifications();
        }
    }
}
