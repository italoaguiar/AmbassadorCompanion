using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Xbox.Ambassadors.Controls
{
    [TemplateVisualState(Name = "Disliked", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "DislikedPointerOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "DislikedDisabled", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "DislikedPressed", GroupName = "CommonStates")]
    public class DislikeButton : Button
    {
        public DislikeButton() : base()
        {
            this.DefaultStyleKey = typeof(DislikeButton);



            _dislikeEventArgs = new DislikeEventArgs();
            _dislikeEventArgs.PropertyChanged += _dislikeEventArgs_PropertyChanged;


        }

        public static readonly DependencyProperty IsDislikedProperty =
            DependencyProperty.Register("IsDisliked", typeof(bool), typeof(DislikeButton), new PropertyMetadata(false, OnIsDislikedChanged));
        private static void OnIsDislikedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var k = (DislikeButton)d;

            if ((bool)e.NewValue == true) k.DislikeCount++;
            else k.DislikeCount--;

            k.CheckVisualStates();
        }
        public bool IsDisliked
        {
            get
            {
                return (bool)this.GetValue(IsDislikedProperty);
            }
            set
            {
                this.SetValue(IsDislikedProperty, value);
            }
        }
        public static readonly DependencyProperty DislikeCountProperty =
            DependencyProperty.Register("DislikeCount", typeof(long), typeof(DislikeButton), new PropertyMetadata(0));
        public long DislikeCount
        {
            get
            {
                return (long)this.GetValue(DislikeCountProperty);
            }
            set
            {

                this.SetValue(DislikeCountProperty, value);


            }
        }

        public static readonly DependencyProperty LabelVisibilityProperty =
            DependencyProperty.Register("LabelVisibility", typeof(Visibility), typeof(DislikeButton), new PropertyMetadata(Visibility.Visible));
        public Visibility LabelVisibility
        {
            get
            {
                return (Visibility)this.GetValue(LabelVisibilityProperty);
            }
            set
            {

                this.SetValue(LabelVisibilityProperty, value);


            }
        }

        private void CheckVisualStates()
        {
            if (IsDisliked)
            {
                if (IsEnabled)
                {
                    if (IsPointerOver && !IsPressed)
                        GoToState("DislikedPointerOver");
                    else if (IsPressed)
                        GoToState("DislikedPressed");
                    else
                        GoToState("Disliked");
                }
                else
                    GoToState("DislikedDisabled");
            }
            else
            {
                if (IsEnabled)
                {
                    if (IsPointerOver && !IsPressed)
                        GoToState("PointerOver");
                    else if (IsPressed)
                        GoToState("Pressed");
                    else
                        GoToState("Normal");
                }
                else
                    GoToState("Disabled");
            }
        }
        private void GoToState(string state)
        {
            try
            {
                VisualStateManager.GoToState(this, state, true);
            }
            catch { }
        }
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);

            CheckVisualStates();
        }
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            CheckVisualStates();
        }
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            if (RequestForDislike != null)
            {
                RequestForDislike(this, _dislikeEventArgs);
            }
        }


        void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            RequestForDislike?.Invoke(this, _dislikeEventArgs);
        }
        void _dislikeEventArgs_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.IsDisliked = _dislikeEventArgs.Success;
        }
        public event EventHandler<DislikeEventArgs> RequestForDislike;
        private DislikeEventArgs _dislikeEventArgs;

    }
    public class DislikeEventArgs : EventArgs, INotifyPropertyChanged
    {
        bool _success;
        public bool Success
        {
            get { return _success; }
            set
            {
                _success = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Success"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

