using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace Xbox.Ambassadors.Services
{
    public class NotificationService
    {
        private NotificationService()
        {

        }

        public static NotificationService Service { get; set; } = new NotificationService();

        public void Notify(string title, string message)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
            {
                new AdaptiveText()
                {
                    Text = title
                },
                new AdaptiveText()
                {
                    Text = message
                }
            }
                    }
                }
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public void Notify(string title, string message, string imageUrl, string launch, ToastActivationType activationType)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title,
                            HintMaxLines = 1
                        },
                        new AdaptiveText()
                        {
                            Text = message
                        }
                    },
                        HeroImage = new ToastGenericHeroImage()
                        {
                            Source = imageUrl
                        }
                    }
                },
                Launch = launch,
                ActivationType = activationType
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }
    }
}
