using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API.Forum;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Xbox.Ambassadors.IncrementalLoading;
using Xbox.Ambassadors.Services;

namespace Xbox.Ambassadors.ViewModels
{
    public class ForumViewModel : ViewModelBase
    {
        public ForumViewModel(FrameworkElement parent) : base(parent)
        {

            RegisterTimeTrigger(UpdateThreads, new TimeSpan(0, 2, 0));

            UpdateViewModel();
        }


        public ForumService ForumService
        {
            get { return ForumService.Service; }
        }


        ForumThread _selectedThread;

        bool _isThreadsLoading = false;
        bool _threadsLoadFailed = false;

        public bool IsThreadsLoading
        {
            get { return _isThreadsLoading; }
            set
            {
                _isThreadsLoading = value;
                RaisePropertyChanged("IsThreadsLoading");
            }
        }

        public bool ThreadsLoadFailed
        {
            get { return _threadsLoadFailed; }
            set
            {
                _threadsLoadFailed = value;
                RaisePropertyChanged("ThreadsLoadFailed");
            }
        }

        public ForumThread SelectedThread
        {
            get => _selectedThread;
            set
            {
                _selectedThread = value;
                RaisePropertyChanged("SelectedThread");
                App.AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("Forum", "Open Thread", SelectedThread.ThreadUri.AbsoluteUri).Build());
            }
        }

        public ForumLoader<ForumThread> Threads
        {
            get => ForumService.Threads;
        }








        private async void UpdateThreads()
        {
            try
            {
                IsThreadsLoading = true;
                ThreadsLoadFailed = false;

                await ForumService.Service.UpdateAsync();



                if (SelectedThread != null)
                    SelectedThread = Threads.FirstOrDefault(x => x.Id == SelectedThread.Id);

                IsThreadsLoading = false;
            }
            catch (Exception e)
            {
                IsThreadsLoading = false;
                ThreadsLoadFailed = true;
                var k = e;
            }
        }

        public override void UpdateViewModel()
        {
            UpdateThreads();
        }
    }
}
