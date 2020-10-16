using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API;
using Microsoft.Xbox.Ambassadors.API.DataModel.Missions;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;
using Xbox.Ambassadors.Services;

namespace Xbox.Ambassadors.ViewModels
{
    public class MissionsViewModel : ViewModelBase
    {
        public MissionsViewModel(FrameworkElement parent) : base(parent)
        {
            this.RegisterTimeTrigger(UpdateMissions, new TimeSpan(0, 10, 0, 0));

            ReportActivityCommand = new CommandAdapter((p) => SubmitActivity()) { CanExecuteAction = false };
            RetryLoadCommand = new CommandAdapter((p) => UpdateMissions());

            UpdateViewModel();
        }

        #region Missions

        bool _isMissionsLoading = false;
        bool _missionsLoadFailed = false;
        bool _hasNoMissions = false;
        bool _hasNoCompletedMissions = false;
        Missions _missionsData;
        CompletedMission[] _completedMissions;
        SelfReportedActivity[] _activities;
        SelfReportedActivity _selectedActivity;


        public bool IsMissionsLoading
        {
            get { return _isMissionsLoading; }
            set
            {
                _isMissionsLoading = value;
                RaisePropertyChanged("IsMissionsLoading");
            }
        }

        public bool MissionsLoadFailed
        {
            get { return _missionsLoadFailed; }
            set
            {
                _missionsLoadFailed = value;
                RaisePropertyChanged("MissionsLoadFailed");
            }
        }

        public Missions AllMissions
        {
            get { return _missionsData; }
            set
            {
                _missionsData = value;
                RaisePropertyChanged("AllMissions");
            }
        }

        public SelfReportedActivity[] Activities
        {
            get => _activities;
            set
            {
                _activities = value;
                RaisePropertyChanged("Activities");
            }
        }

        public SelfReportedActivity SelectedActivity
        {
            get => _selectedActivity;
            set
            {
                _selectedActivity = value;
                RaisePropertyChanged("SelectedActivity");
                ReportActivityCommand.CanExecuteAction = value != null;
            }
        }
        public CommandAdapter ReportActivityCommand { get; set; }
        public CommandAdapter RetryLoadCommand { get; set; }


        public CompletedMission[] CompletedMissions
        {
            get => _completedMissions;
            set
            {
                _completedMissions = value;
                RaisePropertyChanged("CompletedMissions");
            }
        }

        public bool HasNoMissions
        {
            get => _hasNoMissions;
            set
            {
                _hasNoMissions = value;
                RaisePropertyChanged("HasNoMissions");
            }
        }
        public bool HasNoCompletedMissions
        {
            get => _hasNoCompletedMissions;
            set
            {
                _hasNoCompletedMissions = value;
                RaisePropertyChanged("HasNoCompletedMissions");
            }
        }

        private async void SubmitActivity()
        {
            try
            {
                ReportActivityCommand.CanExecuteAction = false;

                var r = await SelfReportedActivity.PostAsync(
                    await AccessToken.LoadFromVaultOrGetNew(),
                    SelectedActivity.ActivityId
                    );

                if (r.IsSuccessStatusCode)
                {
                    NotificationService.Service.Notify("Success", "Self-Reported activity was successfully submitted!");
                }
                else
                {
                    NotificationService.Service.Notify("Error", "Failed to submit activity");
                }

                App.AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("Click", "Missions", "Self-Reported").Build());
            }
            catch
            {
                NotificationService.Service.Notify("Error", "Failed to submit activity");
            }
            finally
            {
                ReportActivityCommand.CanExecuteAction = true;
            }
        }

        private async void UpdateMissions()
        {
            try
            {
                IsMissionsLoading = true;
                MissionsLoadFailed = false;

                AllMissions = await Missions.GetAsync(await AccessToken.LoadFromVaultOrGetNew());

                CompletedMissions = await Missions.GetCompletedAsync(await AccessToken.LoadFromVaultOrGetNew());

                Activities = await SelfReportedActivity.GetAsync(await AccessToken.LoadFromVaultOrGetNew());

                HasNoMissions = AllMissions == null || AllMissions.AllMissions == null || AllMissions.AllMissions.Count() == 0;
                HasNoCompletedMissions = CompletedMissions == null || CompletedMissions.Length == 0;

                IsMissionsLoading = false;
            }
            catch
            {
                MissionsLoadFailed = true;
            }
        }

        #endregion

        public override void UpdateViewModel()
        {
            UpdateMissions();
        }
    }
}
