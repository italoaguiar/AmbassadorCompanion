using System;
using Windows.UI.Xaml.Data;

namespace Xbox.Ambassadors.Converters
{
    public class NullableBooleanConverter : IValueConverter
    {
        public object NullValue { get; set; }
        public object TrueValue { get; set; }
        public object FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool? r = (bool?)value;

            if (r == true)
                return TrueValue;
            else if (r == false)
                return FalseValue;
            else
                return NullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
