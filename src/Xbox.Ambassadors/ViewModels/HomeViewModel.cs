using Microsoft.Xbox.Ambassadors.API;
using Microsoft.Xbox.Ambassadors.API.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;
using Xbox.Ambassadors.Services;

namespace Xbox.Ambassadors.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(FrameworkElement parent) : base(parent)
        {
            this.RegisterTimeTrigger(UpdateSiteSectionsContent, new TimeSpan(1, 0, 0))
                .RegisterTimeTrigger(UpdateMissions, new TimeSpan(0, 0, 20, 0))
                .RegisterTimeTrigger(UpdateThreads, new TimeSpan(0, 1, 0));

            ReloadMissionsCommand = new CommandAdapter((p) => UpdateMissions());
            ReloadThreadsCommand = new CommandAdapter((p) => UpdateThreads());
            ReloadSiteSectionsCommand = new CommandAdapter((p) => UpdateSiteSectionsContent());

            UpdateViewModel();

            ForumService.Service.Threads.CollectionChanged += (s, a) =>
            {
                RaisePropertyChanged(nameof(TopThreads));
            };
        }

        public CommandAdapter ReloadMissionsCommand { get; set; }
        public CommandAdapter ReloadThreadsCommand { get; set; }

        public CommandAdapter ReloadSiteSectionsCommand { get; set; }

        #region Forum Data
        public List<ForumThread> TopThreads
        {
            get { return ForumService.Service.Threads.Where(x => !x.IsLocked).Take(8).ToList(); }
        }

        bool _isThreadsLoading = false;
        bool _threadsLoadFailed = false;

        public bool IsThreadsLoading
        {
            get { return _isThreadsLoading; }
            set
            {
                _isThreadsLoading = value;
                RaisePropertyChanged(nameof(IsThreadsLoading));
            }
        }

        public bool ThreadsLoadFailed
        {
            get { return _threadsLoadFailed; }
            set
            {
                _threadsLoadFailed = value;
                RaisePropertyChanged(nameof(ThreadsLoadFailed));
            }
        }

        private async void UpdateThreads()
        {
            try
            {
                IsThreadsLoading = true;
                ThreadsLoadFailed = false;

                await ForumService.Service.UpdateAsync();

                IsThreadsLoading = false;
            }
            catch (Exception e)
            {
                IsThreadsLoading = false;
                ThreadsLoadFailed = true;
            }
        }
        #endregion

        #region User Data
        public Profiles Profile
        {
            get { return AmbassadorProfile.Profile; }
        }

        public Identity UserIdentity
        {
            get { return AmbassadorProfile.UserIdentity; }
        }
        #endregion

        #region Missions

        bool _isMissionsLoading = false;
        bool _missionsLoadFailed = false;
        Missions _missionsData;

        public bool IsMissionsLoading
        {
            get { return _isMissionsLoading; }
            set
            {
                _isMissionsLoading = value;
                RaisePropertyChanged(nameof(IsMissionsLoading));
            }
        }

        public bool MissionsLoadFailed
        {
            get { return _missionsLoadFailed; }
            set
            {
                _missionsLoadFailed = value;
                RaisePropertyChanged(nameof(MissionsLoadFailed));
            }
        }

        public Missions MissionsData
        {
            get { return _missionsData; }
            set
            {
                _missionsData = value;
                RaisePropertyChanged(nameof(MissionsData));
            }
        }

        private async void UpdateMissions()
        {
            try
            {
                IsMissionsLoading = true;
                MissionsLoadFailed = false;

                var token = await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false);
                MissionsData = await Missions.GetAsync(token);

                IsMissionsLoading = false;
            }
            catch
            {
                MissionsLoadFailed = true;
            }
        }

        #endregion

        #region Content

        bool _isContentLoading = false;
        bool _contentLoadFailed = false;
        List<Story> _siteContent;
        AmbassadorMonth _ambassadorMonth;
        FeaturedActivity _featuredActivity;
        FeaturedBlog _featuredBlog;


        public bool IsContentLoading
        {
            get { return _isContentLoading; }
            set
            {
                _isContentLoading = value;
                RaisePropertyChanged(nameof(IsContentLoading));
            }
        }

        public bool ContentLoadFailed
        {
            get { return _contentLoadFailed; }
            set
            {
                _contentLoadFailed = value;
                RaisePropertyChanged(nameof(ContentLoadFailed));
            }
        }

        public List<Story> Announcements
        {
            get { return _siteContent; }
            set
            {
                _siteContent = value;
                RaisePropertyChanged(nameof(Announcements));
            }
        }

        public AmbassadorMonth AmbassadorOfMonth
        {
            get { return _ambassadorMonth; }
            set
            {
                _ambassadorMonth = value;
                RaisePropertyChanged(nameof(AmbassadorOfMonth));
            }
        }

        public FeaturedActivity FeaturedActivity
        {
            get { return _featuredActivity; }
            set
            {
                _featuredActivity = value;
                RaisePropertyChanged(nameof(FeaturedActivity));
            }
        }

        public FeaturedBlog FeaturedBlog
        {
            get { return _featuredBlog; }
            set
            {
                _featuredBlog = value;
                RaisePropertyChanged(nameof(FeaturedBlog));
            }
        }



        private async void UpdateSiteSectionsContent()
        {
            try
            {
                IsContentLoading = true;
                ContentLoadFailed = false;

                var token = await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false);

                var r = await SiteContent.GetAsync(token,
                    SiteContent.ANNOUNCEMENTS,
                    SiteContent.AMBASSADOR_OF_MONTH,
                    SiteContent.FEATURED_BLOG,
                    SiteContent.FEATURED_ACTIVITY);

                AmbassadorOfMonth = r[SiteContent.AMBASSADOR_OF_MONTH].Document.SectionType.AmbassadorMonth;
                FeaturedBlog = r[SiteContent.FEATURED_BLOG].Document.SectionType.FeaturedBlog;
                FeaturedActivity = r[SiteContent.FEATURED_ACTIVITY].Document.SectionType.FeaturedActivity;

                var Document = r[SiteContent.ANNOUNCEMENTS].Document;



                List<Story> s = new List<Story>();
                if (Document != null && Document.SectionType != null && Document.SectionType.FeaturedStories != null)
                {
                    if (Document.SectionType.FeaturedStories.Story1 != null)
                        s.Add(Document.SectionType.FeaturedStories.Story1);

                    if (Document.SectionType.FeaturedStories.Story2 != null)
                        s.Add(Document.SectionType.FeaturedStories.Story2);

                    if (Document.SectionType.FeaturedStories.Story3 != null)
                        s.Add(Document.SectionType.FeaturedStories.Story3);
                }
                Announcements = s;

                IsContentLoading = false;
            }
            catch
            {
                ContentLoadFailed = true;
                IsContentLoading = false;
            }
        }

        #endregion

        #region Leaderboard
        //bool _isLeaderboardLoading;
        //bool _leaderboardLoadFailed;
        //Leaderboard _leaderboard;

        //public bool IsLeaderboardLoading
        //{
        //    get { return _isLeaderboardLoading; }
        //    set
        //    {
        //        _isLeaderboardLoading = value;
        //        RaisePropertyChanged(nameof(IsLeaderboardLoading));
        //    }
        //}

        //public bool LeaderboardLoadFailed
        //{
        //    get { return _leaderboardLoadFailed; }
        //    set
        //    {
        //        _leaderboardLoadFailed = value;
        //        RaisePropertyChanged(nameof(LeaderboardLoadFailed));
        //    }
        //}

        //public Leaderboard Leaderboard
        //{
        //    get { return _leaderboard; }
        //    set
        //    {
        //        _leaderboard = value;
        //        RaisePropertyChanged(nameof(Leaderboard));
        //    }
        //}

        //private async void UpdateLeaderboard()
        //{
        //    try
        //    {
        //        IsLeaderboardLoading = true;
        //        LeaderboardLoadFailed = false;

        //        var token = await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false);

        //        var s = await Season.GetAsync(token).ConfigureAwait(true);
        //        Leaderboard = await Leaderboard.GetAsync(token, (uint)s.Last().SeasonId, 30, 0).ConfigureAwait(true);

        //        IsLeaderboardLoading = false;
        //    }
        //    catch
        //    {
        //        IsLeaderboardLoading = false;
        //        LeaderboardLoadFailed = true;
        //    }
        //}
        #endregion

        public override void UpdateViewModel()
        {
            UpdateMissions();
            UpdateSiteSectionsContent();
            UpdateThreads();
            //UpdateLeaderboard();
            AmbassadorProfile.UpdateProfile();
            RaisePropertyChanged(nameof(Profile));
        }

    }
}
