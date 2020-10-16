using Microsoft.Xbox.Live;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Xbox.Ambassadors.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Status s = value as Status;
            if (s != null)
            {
                if (s.Id == "1")
                    return parameter == null ? Visibility.Visible : Visibility.Collapsed;
                else
                {
                    return parameter == null ? Visibility.Collapsed : Visibility.Visible;
                }
            }


            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
