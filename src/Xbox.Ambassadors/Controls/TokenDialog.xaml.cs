using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Controls
{
    public sealed partial class TokenDialog : ContentDialog
    {
        public TokenDialog()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty TokenProperty =
            DependencyProperty.Register("Token", typeof(string), typeof(TokenDialog),
                new PropertyMetadata(string.Empty));


        public string Token
        {
            get => (string)GetValue(TokenProperty);
            set => SetValue(TokenProperty, value);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
