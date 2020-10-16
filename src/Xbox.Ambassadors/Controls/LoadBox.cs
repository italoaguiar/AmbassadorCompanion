using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Xbox.Ambassadors.Controls
{
    [DefaultProperty("Content")]
    [ContentProperty(Name = "Content")]
    public sealed class LoadBox : Control
    {
        public LoadBox()
        {
            this.DefaultStyleKey = typeof(LoadBox);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            CheckVisualStates();
        }




        public static readonly DependencyProperty ReloadCommandProperty =
            DependencyProperty.Register("ReloadCommand", typeof(ICommand), typeof(LoadBox), new PropertyMetadata(null));

        public ICommand ReloadCommand
        {
            get
            {
                return (ICommand)this.GetValue(ReloadCommandProperty);
            }
            set
            {

                this.SetValue(ReloadCommandProperty, value);
            }
        }



        public static readonly DependencyProperty NetworkErrorTemplateProperty =
            DependencyProperty.Register("NetworkErrorTemplate", typeof(ControlTemplate), typeof(LoadBox), new PropertyMetadata(null));

        public ControlTemplate NetworkErrorTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(NetworkErrorTemplateProperty);
            }
            set
            {

                this.SetValue(NetworkErrorTemplateProperty, value);
            }
        }


        public static readonly DependencyProperty ErrorTemplateProperty =
            DependencyProperty.Register("ErrorTemplate", typeof(ControlTemplate), typeof(LoadBox), new PropertyMetadata(null));

        public ControlTemplate ErrorTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(ErrorTemplateProperty);
            }
            set
            {

                this.SetValue(ErrorTemplateProperty, value);
            }
        }



        public static readonly DependencyProperty NoDataTemplateProperty =
            DependencyProperty.Register("NoDataTemplate", typeof(ControlTemplate), typeof(LoadBox), new PropertyMetadata(null));

        public ControlTemplate NoDataTemplate
        {
            get
            {
                return (ControlTemplate)this.GetValue(NoDataTemplateProperty);
            }
            set
            {

                this.SetValue(NoDataTemplateProperty, value);
            }
        }


        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(LoadBox), new PropertyMetadata(null));

        [Category("Content")]
        public object Content
        {
            get
            {
                return (object)this.GetValue(ContentProperty);
            }
            set
            {

                this.SetValue(ContentProperty, value);
            }
        }


        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(LoadBox), new PropertyMetadata(null));


        public object Header
        {
            get
            {
                return (object)this.GetValue(HeaderProperty);
            }
            set
            {

                this.SetValue(HeaderProperty, value);
            }
        }


        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadBox), new PropertyMetadata(false, OnIsLoadingChanged));

        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadBox)d).CheckVisualStates();
        }

        public bool IsLoading
        {
            get
            {
                return (bool)this.GetValue(IsLoadingProperty);
            }
            set
            {

                this.SetValue(IsLoadingProperty, value);
            }
        }





        public static readonly DependencyProperty HasNetworkErrorProperty =
            DependencyProperty.Register("HasNetworkError", typeof(bool), typeof(LoadBox), new PropertyMetadata(false, OnHasNetworkErrorChanged));

        private static void OnHasNetworkErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadBox)d).CheckVisualStates();
        }

        public bool HasNetworkError
        {
            get
            {
                return (bool)this.GetValue(HasNetworkErrorProperty);
            }
            set
            {

                this.SetValue(HasNetworkErrorProperty, value);
            }
        }






        public static readonly DependencyProperty HasProcessingErrorProperty =
            DependencyProperty.Register("HasProcessingError", typeof(bool), typeof(LoadBox), new PropertyMetadata(false, OnHasProcessingErrorChanged));

        private static void OnHasProcessingErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadBox)d).CheckVisualStates();
        }

        public bool HasProcessingError
        {
            get
            {
                return (bool)this.GetValue(HasProcessingErrorProperty);
            }
            set
            {

                this.SetValue(HasProcessingErrorProperty, value);
            }
        }



        public static readonly DependencyProperty HasNoDataProperty =
            DependencyProperty.Register("HasNoData", typeof(bool), typeof(LoadBox), new PropertyMetadata(false, OnHasNoDataChanged));

        private static void OnHasNoDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LoadBox)d).CheckVisualStates();
        }

        public bool HasNoData
        {
            get
            {
                return (bool)this.GetValue(HasNoDataProperty);
            }
            set
            {

                this.SetValue(HasNoDataProperty, value);
            }
        }


        private void CheckVisualStates()
        {
            if (HasNetworkError)
            {
                VisualStateManager.GoToState(this, "NetworkError", true);
            }
            else if (HasProcessingError)
            {
                VisualStateManager.GoToState(this, "ProcessingError", true);
            }
            else if (HasNoData)
            {
                VisualStateManager.GoToState(this, "NoData", true);
            }
            else if (IsLoading)
            {
                VisualStateManager.GoToState(this, "Loading", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

    }
}
