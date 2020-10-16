using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Xbox.Ambassadors.Controls
{
    public sealed partial class SubmitTicketsDialog : ContentDialog
    {
        public SubmitTicketsDialog()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(long), typeof(SubmitTicketsDialog), new PropertyMetadata(default(long)));

        public long Value
        {
            get
            {
                return (long)this.GetValue(ValueProperty);
            }
            set
            {

                this.SetValue(ValueProperty, value);
            }
        }


        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(long), typeof(SubmitTicketsDialog), new PropertyMetadata(default(long)));

        public long Maximum
        {
            get
            {
                return (long)this.GetValue(MaximumProperty);
            }
            set
            {

                this.SetValue(MaximumProperty, value);
            }
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (Value < Maximum)
                Value++;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Value > 0)
                Value--;
        }
    }
}
