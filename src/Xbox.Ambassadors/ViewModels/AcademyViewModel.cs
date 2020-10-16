using Microsoft.Xbox.Ambassadors.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;
using Xbox.Ambassadors.Helpers;

namespace Xbox.Ambassadors.ViewModels
{
    public class AcademyViewModel : ViewModelBase
    {
        public AcademyViewModel(FrameworkElement parent) : base(parent)
        {

            ReloadCoursesCommand = new CommandAdapter((p) => LoadCoursesAsync());
            //ReloadContentCommand = new CommandAdapter((p) => LoadContentAsync());
            ReloadQuizzesCommand = new CommandAdapter((p) => LoadQuizzesAsync());

            this/*.RegisterTimeTrigger(LoadContentAsync, new TimeSpan(1, 0, 0))*/
                .RegisterTimeTrigger(LoadCoursesAsync, new TimeSpan(0, 10, 0))
                .RegisterTimeTrigger(LoadQuizzesAsync, new TimeSpan(0, 10, 0));

            UpdateViewModel();
        }

        bool _isLoadingContent;
        bool _loadContentFailed;
        //string _title;
        //string _content;

        bool _hasNoCourses;
        bool _isCoursesLoading;
        bool _loadCoursesFailed;
        IList<Course> courses;

        bool _isQuizzesLoading;
        bool _loadQuizzesFailed;
        ObservableCollection<Quizz> _extracurricularQuizzes;
        ObservableCollection<Quizz> _allQuizzes;

        IList<CourseElement> _courseDetails;

        public CommandAdapter ReloadCoursesCommand { get; set; }
        //public CommandAdapter ReloadContentCommand { get; set; }
        public CommandAdapter ReloadQuizzesCommand { get; set; }

        //public string Title
        //{
        //    get { return _title; }
        //    set
        //    {
        //        _title = value;
        //        RaisePropertyChanged("Title");
        //    }
        //}

        //public string Content
        //{
        //    get { return _content; }
        //    set
        //    {
        //        _content = value;
        //        RaisePropertyChanged("Content");
        //    }
        //}

        //public bool IsContentLoading
        //{
        //    get { return _isLoadingContent; }
        //    set
        //    {
        //        _isLoadingContent = value;
        //        RaisePropertyChanged("IsContentLoading");
        //    }
        //}

        //public bool LoadContentFailed
        //{
        //    get { return _loadContentFailed; }
        //    set
        //    {
        //        _loadContentFailed = value;
        //        RaisePropertyChanged("LoadContentFailed");
        //    }
        //}

        public IList<CourseElement> Courses
        {
            get { return _courseDetails; }
            set
            {
                _courseDetails = value;
                RaisePropertyChanged(nameof(Courses));
            }
        }



        public bool IsCoursesLoading
        {
            get { return _isCoursesLoading; }
            set
            {
                _isCoursesLoading = value;
                RaisePropertyChanged(nameof(IsCoursesLoading));
            }
        }

        public bool LoadCoursesFailed
        {
            get { return _loadCoursesFailed; }
            set
            {
                _loadCoursesFailed = value;
                RaisePropertyChanged(nameof(LoadCoursesFailed));
            }
        }

        public bool IsQuizzesLoading
        {
            get { return _isQuizzesLoading; }
            set
            {
                _isQuizzesLoading = value;
                RaisePropertyChanged(nameof(IsQuizzesLoading));
            }
        }

        public bool LoadQuizzesFailed
        {
            get => _loadQuizzesFailed;
            set
            {
                _loadQuizzesFailed = value;
                RaisePropertyChanged(nameof(LoadQuizzesFailed));
            }
        }

        public ObservableCollection<Quizz> ExtracurricularQuizzes
        {
            get => _extracurricularQuizzes;
            set
            {
                _extracurricularQuizzes = value;
                RaisePropertyChanged(nameof(ExtracurricularQuizzes));
            }
        }

        public ObservableCollection<Quizz> AllQuizzes
        {
            get => _allQuizzes;
            set
            {
                _allQuizzes = value;
                RaisePropertyChanged(nameof(AllQuizzes));
            }
        }

        public bool HasNoCourses
        {
            get => _hasNoCourses;
            set
            {
                _hasNoCourses = value;
                RaisePropertyChanged(nameof(HasNoCourses));
            }
        }

        private async void LoadQuizzesAsync()
        {
            try
            {
                IsQuizzesLoading = true;
                LoadQuizzesFailed = false;

                ExtracurricularQuizzes = (await Quizzes.GetAsync(await AccessToken.LoadFromVaultOrGetNew())).ToObservableCollection();
                AllQuizzes = (await Quizzes.GetAsync(await AccessToken.LoadFromVaultOrGetNew(), false)).ToObservableCollection();

                UpdateCourses();

                IsQuizzesLoading = false;
            }
            catch
            {
                IsQuizzesLoading = false;
                LoadQuizzesFailed = true;
            }
        }

        private async void LoadCoursesAsync()
        {
            try
            {
                IsCoursesLoading = true;
                LoadCoursesFailed = false;

                courses = await Microsoft.Xbox.Ambassadors.API.Courses.GetAsync(await AccessToken.LoadFromVaultOrGetNew());

                HasNoCourses = courses == null || courses.Count == 0;

                UpdateCourses();

                IsCoursesLoading = false;
            }
            catch
            {
                IsCoursesLoading = false;
                LoadCoursesFailed = true;
            }
        }

        //private async void LoadContentAsync()
        //{
        //    try
        //    {
        //        IsContentLoading = true;
        //        LoadContentFailed = false;

        //        var c = await SiteContent.GetAsync(await AccessToken.LoadFromVaultOrGetNew(), SiteContent.ACADEMY);
        //        Content = c.Documents[0].Document.Content;
        //        Title = c.Documents[0].Document.Header;

        //        IsContentLoading = false;
        //    }
        //    catch
        //    {
        //        IsContentLoading = false;
        //        LoadContentFailed = true;
        //    }
        //}

        private void UpdateCourses()
        {
            if (AllQuizzes != null && courses != null)
            {
                var c = new CourseElement[courses.Count];

                for (int i = 0; i < courses.Count; i++)
                {
                    CourseElement e = new CourseElement();
                    e.Course = courses[i];
                    foreach (var p in courses[i].QuizResults)
                    {
                        e.Quizzes = new Quizz[courses[i].QuizResults.Count];
                        for (int j = 0; j < e.Quizzes.Count; j++)
                        {
                            e.Quizzes[j] = AllQuizzes.Where(x => x.Id.Equals(courses[i].QuizResults[j].QuizId)).FirstOrDefault();
                        }
                    }
                    c[i] = e;
                }
                Courses = c;
            }
        }

        public override void UpdateViewModel()
        {
            //LoadContentAsync();
            LoadCoursesAsync();
            LoadQuizzesAsync();
        }

    }

    public class CourseElement
    {
        public Course Course { get; set; }
        public IList<Quizz> Quizzes { get; set; }
    }
}
