using Microsoft.Xbox.Ambassadors.API;
using System;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.ViewModels.Profile
{
    public class ProfileBadgesViewModel : ViewModelBase
    {
        public ProfileBadgesViewModel(FrameworkElement parent) : base(parent)
        {

        }

        BadgeSection[] badgeSections;
        Visibility badgeEnabledVisibility = Visibility.Collapsed;
        Visibility badgeDisabledVisibility = Visibility.Collapsed;
        Visibility headerVisibility = Visibility.Collapsed;
        bool isXboxLiveBadgeEnabled;

        public BadgeSection[] BadgeSections
        {
            get => badgeSections;
            set
            {
                badgeSections = value;
                RaisePropertyChanged(nameof(BadgeSections));
            }
        }

        public Visibility BadgeEnabledVisibility
        {
            get => badgeEnabledVisibility;
            set
            {
                badgeEnabledVisibility = value;
                RaisePropertyChanged(nameof(BadgeEnabledVisibility));
            }
        }
        public Visibility BadgeDisabledVisibility
        {
            get => badgeDisabledVisibility;
            set
            {
                badgeDisabledVisibility = value;
                RaisePropertyChanged(nameof(BadgeDisabledVisibility));
            }
        }
        public bool IsXboxLiveBadgeEnabled
        {
            get => isXboxLiveBadgeEnabled;
            set
            {
                isXboxLiveBadgeEnabled = value;
                RaisePropertyChanged(nameof(IsXboxLiveBadgeEnabled));
                SetPreference(value);
            }
        }

        public Visibility HeaderVisibility
        {
            get => headerVisibility;
            set
            {
                headerVisibility = value;
                RaisePropertyChanged(nameof(HeaderVisibility));
            }
        }

        private async void SetPreference(bool value)
        {
            try
            {
                await AmbassadorPreferences.PostAsync(await AccessToken.LoadFromVaultOrGetNew(), value);
                BadgeEnabledVisibility = IsXboxLiveBadgeEnabled ? Visibility.Visible : Visibility.Collapsed;
                BadgeDisabledVisibility = IsXboxLiveBadgeEnabled ? Visibility.Collapsed : Visibility.Visible;
            }
            catch
            {

            }
        }

        private async void LoadSections()
        {
            try
            {

                isXboxLiveBadgeEnabled = AmbassadorProfile.Profile.OptedInToDisplayAmbBadge;
                RaisePropertyChanged(nameof(IsXboxLiveBadgeEnabled));

                BadgeEnabledVisibility = IsXboxLiveBadgeEnabled ? Visibility.Visible : Visibility.Collapsed;
                BadgeDisabledVisibility = IsXboxLiveBadgeEnabled ? Visibility.Collapsed : Visibility.Visible;


                var badges = new BadgeSection[2];

                BadgeSection s = new BadgeSection();
                s.Name = "Badges";
                s.Items = new BadgeSectionItem[AmbassadorProfile.Profile.DynamicBadges.Count];
                for (int i = 0; i < AmbassadorProfile.Profile.DynamicBadges.Count; i++)
                {
                    BadgeSectionItem item = new BadgeSectionItem();
                    item.Name = AmbassadorProfile.Profile.DynamicBadges[i].DisplayName;
                    item.Description = AmbassadorProfile.Profile.DynamicBadges[i].DisplayDescription;
                    item.Image = AmbassadorProfile.Profile.DynamicBadges[i].IconUri;
                    s.Items[i] = item;
                }

                BadgeSection s2 = new BadgeSection();
                s2.Name = "Pins";
                s2.Items = new BadgeSectionItem[AmbassadorProfile.Profile.Badges.Count];
                for (int i = 0; i < AmbassadorProfile.Profile.Badges.Count; i++)
                {
                    BadgeSectionItem item = new BadgeSectionItem();
                    item.Name = AmbassadorProfile.Profile.Badges[i].Title;
                    item.Description = AmbassadorProfile.Profile.Badges[i].SubTitle;
                    item.Image = AmbassadorProfile.Profile.Badges[i].ImageUrl;
                    s2.Items[i] = item;
                }
                badges[0] = s;
                badges[1] = s2;
                BadgeSections = badges;

                if (AmbassadorProfile.Profile.NextTierId != 5887)
                    HeaderVisibility = Visibility.Visible;
            }
            catch
            {

            }
        }

        public override void UpdateViewModel()
        {
            LoadSections();
        }
    }
    public class BadgeSection
    {
        public string Name { get; set; }
        public BadgeSectionItem[] Items { get; set; }
    }
    public class BadgeSectionItem
    {
        public Uri Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
