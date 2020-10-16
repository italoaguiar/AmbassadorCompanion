using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;
using Xbox.Ambassadors.Services;

namespace Xbox.Ambassadors.ViewModels
{
    public class QuizzViewModel : ViewModelBase
    {
        public QuizzViewModel(Views.Quizz parent) : base(parent)
        {
            QuizzPage = parent;

            SubmitCommand = new CommandAdapter((p) => SubmitQuizzAsync());
            RetryLoadCommand = new CommandAdapter((p) => LoadQuizzAsync());
            RetakeCommand = new CommandAdapter((p) => LoadQuizzAsync());
        }

        Views.Quizz QuizzPage;


        bool _isQuizzLoading;
        bool _loadQuizzFailed;
        bool _interactionEnabled = true;
        bool _resultBoxVisible = false;


        public Quizz Quizz
        {
            get => QuizzPage.SelectedQuizz;
            set
            {
                QuizzPage.SelectedQuizz = value;
                RaisePropertyChanged(nameof(Quizz));
                RaisePropertyChanged(nameof(QuestionCount));
                RaisePropertyChanged(nameof(TotalCorrect));
                RaisePropertyChanged(nameof(QuizzPassed));
            }
        }

        public CommandAdapter SubmitCommand { get; set; }
        public CommandAdapter RetryLoadCommand { get; set; }
        public CommandAdapter RetakeCommand { get; set; }

        public bool QuizzPassed
        {
            get { return Quizz.Score >= Quizz.PassingScore; }
        }

        public bool IsQuizzLoading
        {
            get => _isQuizzLoading;
            set
            {
                _isQuizzLoading = value;
                RaisePropertyChanged(nameof(IsQuizzLoading));
            }
        }
        public bool LoadQuizzFailed
        {
            get => _loadQuizzFailed;
            set
            {
                _loadQuizzFailed = value;
                RaisePropertyChanged(nameof(LoadQuizzFailed));
            }
        }

        public bool InteractionEnabled
        {
            get => _interactionEnabled;
            set
            {
                _interactionEnabled = value;
                RaisePropertyChanged(nameof(InteractionEnabled));
            }
        }

        public int QuestionCount
        {
            get => Quizz != null ? Quizz.Questions.Count : 0;
        }

        public int TotalCorrect
        {
            get => Quizz != null ? Quizz.Questions.Count(x => x.IsCorrect == true) : 0;
        }
        public bool ResultBoxVisible
        {
            get
            {
                if (Quizz.Score != null)
                {
                    _resultBoxVisible = true;
                }
                return _resultBoxVisible; ;
            }
            set
            {
                _resultBoxVisible = value;
                RaisePropertyChanged(nameof(ResultBoxVisible));
            }
        }

        public async void LoadQuizzAsync()
        {
            try
            {
                IsQuizzLoading = true;
                LoadQuizzFailed = false;
                ResultBoxVisible = false;

                Quizz = await Quizzes.GetAsync(await AccessToken.LoadFromVaultOrGetNew(), Quizz.Id.ToString());

                IsQuizzLoading = false;
                InteractionEnabled = true;
                SubmitCommand.CanExecuteAction = true;
            }
            catch
            {
                IsQuizzLoading = false;
                LoadQuizzFailed = true;
            }
        }

        private async void SubmitQuizzAsync()
        {
            try
            {
                SubmitCommand.CanExecuteAction = false;
                InteractionEnabled = false;

                Quizz = await Quizzes.PostAsync(await AccessToken.LoadFromVaultOrGetNew(), Quizz);

                ResultBoxVisible = true;
                QuizzSubmited?.Invoke(this, null);

                App.AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("Quizz", "Submit Quizz", Quizz.Id.ToString()).Build());

            }
            catch
            {
                NotificationService.Service.Notify("Error", "Could not send quizz. Try again.");

                SubmitCommand.CanExecuteAction = true;
                InteractionEnabled = true;
            }
        }

        public override void UpdateViewModel()
        {
            //nop
        }

        public event EventHandler QuizzSubmited;
    }
}
