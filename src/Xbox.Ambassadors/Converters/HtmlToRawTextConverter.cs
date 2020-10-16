using HtmlAgilityPack;
using System;
using Windows.UI.Xaml.Data;

namespace Xbox.Ambassadors.Converters
{
    public class HtmlToRawTextConverter : IValueConverter
    {
        public HtmlToRawTextConverter()
        {
            Document = new HtmlDocument();
        }

        private HtmlDocument Document;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Document.LoadHtml(value.ToString());
            return Document.DocumentNode.InnerText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
