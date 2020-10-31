using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using Microsoft.Xbox.Ambassadors.API;
using System.Linq;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.ViewModels.Profile
{
    public class ProfileOverviewViewModel : ViewModelBase
    {
        public ProfileOverviewViewModel(FrameworkElement parent) : base(parent)
        {

        }

        SeriesCollection seasonXPChartData;
        SeriesCollection seasonActivitiesChartData;
        SeriesCollection lifeTimeComparisonChartData;
        SeriesCollection lifetimeXP;
        SeriesCollection lifetimeActivities;
        string[] lifeTimeComparisonChartLabels;
        TierProgress[] seasonProgress;
        long seasonActivityCount;
        long seasonXpCount;
        long activityCount;
        long xpCount;

        public Profiles Profile
        {
            get
            {
                return AmbassadorProfile.Profile;
            }
        }
        public Identity Identity
        {
            get
            {
                return AmbassadorProfile.UserIdentity;
            }
        }

        public SeriesCollection SeasonXPChartData
        {
            get => seasonXPChartData;
            set
            {
                seasonXPChartData = value;
                RaisePropertyChanged(nameof(SeasonXPChartData));
            }
        }

        public SeriesCollection SeasonActivitiesChartData
        {
            get => seasonActivitiesChartData;
            set
            {
                seasonActivitiesChartData = value;
                RaisePropertyChanged(nameof(SeasonActivitiesChartData));
            }
        }

        public SeriesCollection LifeTimeComparisonChartData
        {
            get => lifeTimeComparisonChartData;
            set
            {
                lifeTimeComparisonChartData = value;
                RaisePropertyChanged(nameof(LifeTimeComparisonChartData));
            }
        }

        public string[] LifeTimeComparisonChartLabels
        {
            get => lifeTimeComparisonChartLabels;
            set
            {
                lifeTimeComparisonChartLabels = value;
                RaisePropertyChanged(nameof(LifeTimeComparisonChartLabels));
            }
        }

        public TierProgress[] SeasonProgress
        {
            get => seasonProgress;
            set
            {
                seasonProgress = value;
                RaisePropertyChanged(nameof(SeasonProgress));
            }
        }

        public long SeasonActivityCount
        {
            get => seasonActivityCount;
            set
            {
                seasonActivityCount = value;
                RaisePropertyChanged(nameof(SeasonActivityCount));
            }
        }
        public long SeasonXpCount
        {
            get => seasonXpCount;
            set
            {
                seasonXpCount = value;
                RaisePropertyChanged(nameof(SeasonXpCount));
            }
        }
        public long ActivityCount
        {
            get => activityCount;
            set
            {
                activityCount = value;
                RaisePropertyChanged(nameof(ActivityCount));
            }
        }
        public long XpCount
        {
            get => xpCount;
            set
            {
                xpCount = value;
                RaisePropertyChanged(nameof(XpCount));
            }
        }

        public SeriesCollection LifetimeXP
        {
            get => lifetimeXP;
            set
            {
                lifetimeXP = value;
                RaisePropertyChanged(nameof(LifetimeXP));
            }
        }
        public SeriesCollection LifetimeActivities
        {
            get => lifetimeActivities;
            set
            {
                lifetimeActivities = value;
                RaisePropertyChanged(nameof(LifetimeActivities));
            }
        }

        private void PrepareSeasonXPChart()
        {
            //var s = await Season.GetAsync(await AccessToken.LoadFromVaultOrGetNew());
            //long seasonId = s.Last().SeasonId;

            var currentSeason = Profile.SeasonStatistics.OrderByDescending(x => x.SeasonId).First();

            SeasonActivityCount = currentSeason.TotalActivitiesCompleted;
            SeasonXpCount = currentSeason.XpEarned;

            ActivityCount = Profile.TotalActivitiesCompleted;
            XpCount = Profile.TotalXp;

            CalculeSeasonProgress(SeasonXpCount);


            SeriesCollection c = new SeriesCollection();
            SeriesCollection ca = new SeriesCollection();

            foreach (var i in currentSeason.Channels)
            {
                PieSeries serie = new PieSeries()
                {
                    Title = i.Description,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(i.XpEarned) },
                    DataLabels = true
                };
                ColumnSeries serie2 = new ColumnSeries()
                {
                    Title = i.Description,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(i.CompletedActivityCount) },
                    DataLabels = true, 
                    MaxColumnWidth = 100
                };

                c.Add(serie);
                ca.Add(serie2);
            }
            SeasonXPChartData = c;
            SeasonActivitiesChartData = ca;


            SeriesCollection lifetimeXP = new SeriesCollection();
            SeriesCollection lifetimeAC = new SeriesCollection();
            foreach (var i in Profile.SeasonStatistics.Last().Channels)
            {
                PieSeries serie = new PieSeries()
                {
                    Title = i.Description,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(i.XpEarned) },
                    DataLabels = true
                };
                PieSeries serie2 = new PieSeries()
                {
                    Title = i.Description,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(i.CompletedActivityCount) },
                    DataLabels = true
                };
                lifetimeXP.Add(serie);
                lifetimeAC.Add(serie2);
            }
            LifetimeXP = lifetimeXP;
            LifetimeActivities = lifetimeAC;


            ChartValues<long> valXp = new ChartValues<long>();
            ChartValues<long> valAc = new ChartValues<long>();
            foreach (var i in Profile.SeasonStatistics.Where(a => a.SeasonId != -1))
            {
                valXp.Add(i.XpEarned);
                valAc.Add(i.TotalActivitiesCompleted);
            }
            SeriesCollection tc = new SeriesCollection();
            LineSeries lnXp = new LineSeries();
            LineSeries lnAc = new LineSeries();
            lnXp.Title = "XP";
            lnXp.Values = valXp;
            lnAc.Title = "Activities";
            lnAc.Values = valAc;
            tc.Add(lnXp);
            tc.Add(lnAc);

            LifeTimeComparisonChartData = tc;

            LifeTimeComparisonChartLabels = Profile.SeasonStatistics.Where(a => a.SeasonId != -1).Select(x => x.SeasonName).ToArray();
        }

        private async void CalculeSeasonProgress(long xpEarned)
        {
            var r = await Tiers.GetAsync(await AccessToken.LoadFromVaultOrGetNew());

            SeasonProgress = new TierProgress[r.Length - 1];

            for (int i = 0; i < r.Length - 1; i++)
            {
                TierProgress t = new TierProgress();
                t.Name = r[i].DisplayName;

                if (r[i].MaxXp < xpEarned)
                    t.Value = 100;
                else
                {
                    if (i == 0)
                    {
                        t.Value = xpEarned * 100 / (long)r[i].MaxXp;
                    }
                    else
                    {
                        t.Value = (xpEarned - r[i].MinXp) * 100 / (long)(r[i].MaxXp - r[i].MinXp);
                    }
                }
                SeasonProgress[i] = t;
            }

        }


        public override void UpdateViewModel()
        {
            PrepareSeasonXPChart();
            AmbassadorProfile.UpdateProfile();
            RaisePropertyChanged("Profile");
        }
    }

    public class TierProgress
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
