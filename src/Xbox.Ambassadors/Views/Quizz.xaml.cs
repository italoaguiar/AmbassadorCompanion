using GoogleAnalytics;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xbox.Ambassadors.Services;
using Xbox.Ambassadors.ViewModels;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace Xbox.Ambassadors.Views
{
    public sealed partial class Quizz : Page, INavigableTo, INotifyPropertyChanged
    {
        public Quizz()
        {
            this.InitializeComponent();
            ViewModel = new QuizzViewModel(this);
            ViewModel.QuizzSubmited += ViewModel_QuizzSubmited;

            this.Loaded += Quizz_Loaded;
        }

        private async void Quizz_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SelectedQuizz = (Microsoft.Xbox.Ambassadors.API.Quizz)e.Parameter;

            App.AnalyticsTracker.Send(HitBuilder.CreateScreenView("Quizz").Build());
        }

        private void ViewModel_QuizzSubmited(object sender, EventArgs e)
        {
            resultContainer.StartBringIntoView(new BringIntoViewOptions()
            {
                AnimationDesired = true,
                VerticalOffset = 40
            });
            Score = ViewModel.Quizz.Score;
        }

        public QuizzViewModel ViewModel { get; set; }


        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(object), typeof(Quizz),
                new PropertyMetadata(null));

        public long? Score
        {
            get
            {
                return (long?)this.GetValue(ScoreProperty);
            }
            set
            {

                this.SetValue(ScoreProperty, value);
            }
        }


        public static readonly DependencyProperty SelectedQuizzProperty =
            DependencyProperty.Register("SelectedQuizz", typeof(Microsoft.Xbox.Ambassadors.API.Quizz), typeof(Quizz),
                new PropertyMetadata(null));

        

        public Microsoft.Xbox.Ambassadors.API.Quizz SelectedQuizz
        {
            get
            {
                return (Microsoft.Xbox.Ambassadors.API.Quizz)this.GetValue(SelectedQuizzProperty);
            }
            set
            {

                this.SetValue(SelectedQuizzProperty, value);
            }
        }





        public void RefreshModel()
        {
            ViewModel.UpdateViewModel();
        }

        public ViewModelBase GetViewModel()
        {
            return ViewModel;
        }


        public static T FindControl<T>(UIElement parent) where T : FrameworkElement
        {

            if (parent == null) return null;

            if (parent.GetType() is T)
            {
                return (T)parent;
            }
            T result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);

                if (FindControl<T>(child) != null)
                {
                    result = FindControl<T>(child);
                    break;
                }
            }
            return result;
        }
        private FrameworkElement elementToFocus;
        public FrameworkElement ElementToFocus
        {
            get => elementToFocus;
            set
            {
                elementToFocus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ElementToFocus"));
                TryFocusItem();
            }
        }
        private async void TryFocusItem()=> await FocusManager.TryFocusAsync(ElementToFocus, FocusState.Programmatic);

        private void ElementLoaded(object sender, RoutedEventArgs e)
        {
            if (ElementToFocus == null)
                ElementToFocus = (FrameworkElement)sender;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void ScrollViewer_NoFocusCandidateFound(UIElement sender, NoFocusCandidateFoundEventArgs args)
        {
            if (ElementToFocus != null) TryFocusItem();
        }
    }
}
