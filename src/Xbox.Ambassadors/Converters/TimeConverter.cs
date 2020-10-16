using Microsoft.Xbox.Ambassadors.API.DataModel.Missions;
using System;
using Windows.UI.Xaml.Data;

namespace Xbox.Ambassadors.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeLeft t = value as TimeLeft;

            string useLeft = parameter as string;

            string left = useLeft != null ? useLeft : "";

            if (t != null)
            {
                if (t.Minutes > 0)
                    return $"{t.Minutes} Minutes {left}";
                else if (t.Hours > 0)
                    return $"{t.Hours} Hours  {left}";
                else if (t.Days > 0 && t.Days < 365)
                    return $"{t.Days} Days  {left}";

            }
            return "No time limit";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
