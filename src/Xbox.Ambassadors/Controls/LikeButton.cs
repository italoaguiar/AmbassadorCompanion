using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Xbox.Ambassadors.Controls
{

    [TemplateVisualState(Name = "Liked", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "LikedPointerOver", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "LikedDisabled", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "LikedPressed", GroupName = "CommonStates")]
    public class LikeButton : Button
    {
        public LikeButton()
        {
            this.DefaultStyleKey = typeof(LikeButton);

            _likeEventArgs = new LikeEventArgs();
            _likeEventArgs.PropertyChanged += _likeEventArgs_PropertyChanged;

        }

        public static readonly DependencyProperty IsLikedProperty =
            DependencyProperty.Register("IsLiked", typeof(bool), typeof(LikeButton), new PropertyMetadata(false, OnIsLikedChanged));

        private static void OnIsLikedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var k = (LikeButton)d;

            if ((bool)e.NewValue == true) k.LikeCount++;
            else k.LikeCount--;


            k.CheckVisualStates();
        }

        private void CheckVisualStates()
        {
            if (IsLiked)
            {
                if (IsEnabled)
                {
                    if (IsPointerOver && !IsPressed)
                        GoToState("LikedPointerOver");
                    else if (IsPressed)
                        GoToState("LikedPressed");
                    else
                        GoToState("Liked");
                }
                else
                    GoToState("LikedDisabled");
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

        public bool IsLiked
        {
            get
            {
                return (bool)this.GetValue(IsLikedProperty);
            }
            set
            {
                this.SetValue(IsLikedProperty, value);
            }
        }
        public static readonly DependencyProperty LikeCountProperty =
            DependencyProperty.Register("LikeCount", typeof(long), typeof(LikeButton), new PropertyMetadata(default(long), new PropertyChangedCallback(OnLikeCountChanged)));

        private static void OnLikeCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if ((long)e.NewValue > 0)
                    ((LikeButton)d).LabelVisibility = Visibility.Visible;
            }
            catch { }
        }

        public long LikeCount
        {
            get
            {
                return (long)this.GetValue(LikeCountProperty);
            }
            set
            {

                this.SetValue(LikeCountProperty, value);


            }
        }

        public static readonly DependencyProperty LabelVisibilityProperty =
            DependencyProperty.Register("LabelVisibility", typeof(Visibility), typeof(LikeButton), new PropertyMetadata(Visibility.Collapsed));
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
            RequestForLike?.Invoke(this, _likeEventArgs);
        }

        void _likeEventArgs_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.IsLiked = _likeEventArgs.Success;
        }
        public event EventHandler<LikeEventArgs> RequestForLike;
        private LikeEventArgs _likeEventArgs;

    }
    public class LikeEventArgs : EventArgs, INotifyPropertyChanged
    {
        bool success;
        public bool Success
        {
            get { return success; }
            set
            {
                success = value;

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Success"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
