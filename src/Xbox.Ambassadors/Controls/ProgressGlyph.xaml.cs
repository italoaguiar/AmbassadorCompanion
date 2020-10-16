using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace Xbox.Ambassadors.Controls
{
    public sealed partial class ProgressGlyph : UserControl
    {
        public ProgressGlyph()
        {
            this.InitializeComponent();
        }

        private void UpdateState()
        {
            if (Value == null)
            {
                mediumContainer.Visibility = fullContainer.Visibility = starContainer.Visibility = Visibility.Collapsed;
                return;
            }

            mediumContainer.Visibility = Value < 100 && PassingValue > Value ? Visibility.Visible : Visibility.Collapsed;
            starContainer.Visibility = Value >= 100 ? Visibility.Visible : Visibility.Collapsed;
            fullContainer.Visibility = PassingValue <= Value ? Visibility.Visible : Visibility.Collapsed;
            txtVal.Text = txtVal2.Text = Value.ToString();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(ProgressGlyph),
                new PropertyMetadata(null, new PropertyChangedCallback(OnValueChanged)));


        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProgressGlyph pg = d as ProgressGlyph;
            pg.UpdateState();
        }

        public long? Value
        {
            get
            {
                return (long?)this.GetValue(ValueProperty);
            }
            set
            {

                this.SetValue(ValueProperty, value);
            }
        }




        public static readonly DependencyProperty PassingValueProperty =
            DependencyProperty.Register("PassingValue", typeof(long), typeof(ProgressGlyph),
                new PropertyMetadata(default(long), new PropertyChangedCallback(OnPassingValueChanged)));


        private static void OnPassingValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProgressGlyph pg = d as ProgressGlyph;
            pg.UpdateState();
        }

        public long PassingValue
        {
            get
            {
                return (long)this.GetValue(PassingValueProperty);
            }
            set
            {

                this.SetValue(PassingValueProperty, value);
            }
        }
    }
}
