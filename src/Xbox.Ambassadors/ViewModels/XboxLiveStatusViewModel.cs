using Microsoft.Xbox.Live;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Xbox.Ambassadors.ViewModels
{
    public class XboxLiveStatusViewModel : ViewModelBase
    {
        public XboxLiveStatusViewModel(FrameworkElement parent) : base(parent)
        {
            RegisterTimeTrigger(UpdateViewModel, new TimeSpan(0, 15, 0));
            RetryLoadCommand = new CommandAdapter((p) => UpdateViewModel());

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 1, 0);
            dt.Tick += (s, a) => UpdateViewModel();
            dt.Start();

            UpdateViewModel();
        }

        bool _isLoading;
        bool _loadFailed;
        List<StatusSection> xboxLiveStatus;

        public List<StatusSection> XboxLiveStatus
        {
            get => xboxLiveStatus;
            set
            {
                xboxLiveStatus = value;
                RaisePropertyChanged("XboxLiveStatus");
            }
        }

        public CommandAdapter RetryLoadCommand { get; set; }
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }
        public bool LoadFailed
        {
            get => _loadFailed;
            set
            {
                _loadFailed = value;
                RaisePropertyChanged("LoadFailed");
            }
        }

        public override async void UpdateViewModel()
        {
            try
            {
                IsLoading = true;
                LoadFailed = false;

                var status = await ServiceStatus.GetStatusAsync();

                List<StatusSection> sections = new List<StatusSection>()
                {
                    new StatusSection()
                    {
                        Name = "Services",
                        Entries = new List<StatusEntry>()
                        {
                            new StatusEntry()
                            {
                                Name = "Xbox Live Core Services",
                                Description = "Signing in; creating, managing, or recovering an account; search",
                                Status = status.CoreServices["Xbox Live Core Services"].Status,
                                ErrorMessage = status.CoreServices["Xbox Live Core Services"].Scenarios?.Scenario?.Incidents?.Incident?.Stage?.Message
                            },
                            new StatusEntry()
                            {
                                Name = "Purchase and Content Usage",
                                Description = "Buying items, redeeming codes, or downloading purchases",
                                Status = status.CoreServices["Purchase and Content Usage"].Status,
                                ErrorMessage = status.CoreServices["Purchase and Content Usage"].Scenarios?.Scenario?.Incidents?.Incident?.Stage?.Message
                            },
                            new StatusEntry()
                            {
                                Name = "TV, Music and Video",
                                Description = "Live TV; Groove or Movies & TV video store, including browsing, purchasing, downloading, and streaming",
                                Status = status.CoreServices["TV, Music and Video"].Status,
                                ErrorMessage = status.CoreServices["TV, Music and Video"].Scenarios?.Scenario?.Incidents?.Incident?.Stage?.Message
                            },
                            new StatusEntry()
                            {
                                Name = "Social and Gaming",
                                Description = "In-game matchmaking, cloud storage, finding friends, Game DVR, leaderboards, avatar editing, or pictures",
                                Status = status.CoreServices["Social and Gaming"].Status,
                                ErrorMessage = status.CoreServices["Social and Gaming"].Scenarios?.Scenario?.Incidents?.Incident?.Stage?.Message
                            }
                        }
                    },
                    new StatusSection()
                    {
                        Name = "Apps",
                        Entries = new List<StatusEntry>()
                        {
                            new StatusEntry()
                            {
                                Name = "Xbox One",
                                Status = status.Apps["Xbox One"].Status,
                                ErrorMessage = status.Apps["Xbox One"].Scenarios?.Scenario?.Incidents?.Incident?.Stage?.Message
                            },
                            new StatusEntry()
                            {
                                Name = "Xbox 360",
                                Status = status.Apps["Xbox 360"].Status,
                                ErrorMessage = status.Apps["Xbox 360"].Scenarios?.Scenario?.Incidents?.Incident?.Stage?.Message
                            }
                        }
                    },
                    new StatusSection()
                    {
                        Name = "Games",
                        Entries = new List<StatusEntry>()
                        {
                            new StatusEntry()
                            {
                                Name = "Xbox One",
                                Status = status.Games["Xbox One"].Status,
                                ErrorMessage = status.Games["Xbox One"].Scenarios?.Scenario?.Incidents?.Incident?.Stage?.Message
                            },
                            new StatusEntry()
                            {
                                Name = "Xbox 360",
                                Status = status.Games["Xbox 360"].Status,
                                ErrorMessage = status.Games["Xbox 360"].Scenarios?.Scenario?.Incidents?.Incident?.Stage?.Message
                            }
                        }
                    },
                    new StatusSection()
                    {
                        Name = "Website",
                        Entries = new List<StatusEntry>()
                        {
                            new StatusEntry()
                            {
                                Name = "Website",
                                Description = "Web search, forums, help content, or Xbox.com availability",
                                Status = status.Website.Status
                            }
                        }
                    }
                };

                XboxLiveStatus = sections;

                IsLoading = false;
            }
            catch
            {
                IsLoading = false;
                LoadFailed = true;
            }
        }
    }

    public class StatusSection
    {
        public string Name { get; set; }
        public List<StatusEntry> Entries { get; set; }
    }
    public class StatusEntry
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }

        public string ErrorMessage { get; set; }
    }
}
