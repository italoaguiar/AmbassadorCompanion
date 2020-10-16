using Microsoft.Xbox.Ambassadors.API;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(FrameworkElement parent) : base(parent)
        {

        }

        public Identity UserIdentity
        {
            get { return AmbassadorProfile.UserIdentity; }
        }

        public Profiles Profile
        {
            get { return AmbassadorProfile.Profile; }
        }

        bool _isNotificantionsLoading = false;
        bool _NotificationLoadFailed = false;
        Notification[] _notificationData;

        public bool IsNotificationsLoading
        {
            get { return _isNotificantionsLoading; }
            set
            {
                _isNotificantionsLoading = value;
                RaisePropertyChanged("IsNotificationsLoading");
            }
        }

        public bool NotificationsLoadFailed
        {
            get { return _NotificationLoadFailed; }
            set
            {
                _NotificationLoadFailed = value;
                RaisePropertyChanged("NotificationsLoadFailed");
            }
        }

        public Notification[] NotificationsData
        {
            get { return _notificationData; }
            set
            {
                _notificationData = value;
                RaisePropertyChanged("NotificationsData");
            }
        }

        private async void UpdateNotifications()
        {
            try
            {
                NotificationsLoadFailed = false;
                IsNotificationsLoading = true;

                NotificationsData = await Notifications.GetAsync(await AccessToken.LoadFromVaultOrGetNew());

                IsNotificationsLoading = false;
            }
            catch
            {
                NotificationsLoadFailed = true;
            }
        }

        public override void UpdateViewModel()
        {

        }
    }
}
