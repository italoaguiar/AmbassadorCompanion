using GoogleAnalytics;
using Microsoft.Xbox.Ambassadors.API;
using Microsoft.Xbox.Ambassadors.API.YouTube.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.UI.Composition.Interactions;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;
using Xbox.Ambassadors.Services;
using YouTube.API.Util;

namespace Xbox.Ambassadors.ViewModels
{
    public class VideoGalleryViewModel : ViewModelBase
    {
        public VideoGalleryViewModel(FrameworkElement parent) : base(parent)
        {
            RetryLoadGalleryCommand = new CommandAdapter((p) => LoadGallery());
            DownVoteCommand = new CommandAdapter((p) => DownVote(SelectedReason)) { CanExecuteAction = false };
            VoteCommand = new CommandAdapter((p) => Vote()) { CanExecuteAction = false };
            SubmitVideoCommand = new CommandAdapter((p) => SubmitVideo()) { CanExecuteAction = false };

            LoadGallery();
        }

        bool isGalleryLoading;
        bool isVideosLoading;
        Visibility galleryFailedAlert;
        Visibility videosFailedAlert;
        int galleryVideosCount = 0;
        YouTubeIncrementalLoader<GalleryResponse, VideosParameters> gVideos;
        GalleryItem selectedItem;
        string selectedReason;
        bool isGalleryControlsEnabled;

        Microsoft.Xbox.Ambassadors.API.YouTube.SearchResponseModels.Item[] searchItems;


        public CommandAdapter RetryLoadGalleryCommand { get; set; }
        public CommandAdapter DownVoteCommand { get; set; }
        public CommandAdapter VoteCommand { get; set; }
        public CommandAdapter SubmitVideoCommand { get; set; }

        public bool IsGalleryLoading
        {
            get => isGalleryLoading;
            set
            {
                isGalleryLoading = value;
                RaisePropertyChanged(nameof(IsGalleryLoading));
            }
        }
        public bool IsVideosLoading
        {
            get => isVideosLoading;
            set
            {
                isVideosLoading = value;
                RaisePropertyChanged(nameof(IsVideosLoading));
            }
        }

        public Visibility GalleryFailedAlertVisibility
        {
            get => galleryFailedAlert;
            set
            {
                galleryFailedAlert = value;
                RaisePropertyChanged(nameof(GalleryFailedAlertVisibility));
            }
        }

        public Visibility VideosFailedAlertVisibility
        {
            get => videosFailedAlert;
            set
            {
                videosFailedAlert = value;
                RaisePropertyChanged(nameof(VideosFailedAlertVisibility));
            }
        }

        public YouTubeIncrementalLoader<GalleryResponse, VideosParameters> GalleryVideos
        {
            get => gVideos;
            set
            {
                gVideos = value;
                RaisePropertyChanged(nameof(GalleryVideos));
            }
        }

        public GalleryItem SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                VoteCommand.CanExecuteAction = value != null;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        public string SelectedReason
        {
            get => selectedReason;
            set
            {
                selectedReason = value;
                DownVoteCommand.CanExecuteAction = value != null && selectedItem != null;
                RaisePropertyChanged(nameof(SelectedReason));
            }
        }

        public bool IsGalleryControlsEnabled
        {
            get => isGalleryControlsEnabled;
            set
            {
                isGalleryControlsEnabled = value;
                RaisePropertyChanged(nameof(IsGalleryControlsEnabled));
            }
        }

        public Microsoft.Xbox.Ambassadors.API.YouTube.SearchResponseModels.Item[] SearchItems
        {
            get => searchItems;
            set
            {
                searchItems = value;
                RaisePropertyChanged(nameof(SearchItems));
            }
        }


        private void LoadGallery()
        {
            try
            {
                GalleryFailedAlertVisibility = Visibility.Collapsed;
                IsGalleryLoading = true;





                GalleryVideos = new YouTubeIncrementalLoader<GalleryResponse, VideosParameters>(
                    async (p) =>
                    {
                        try
                        {
                            var g = await Videos.GetAsync(
                                await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false), 
                                20, 
                                (uint)galleryVideosCount
                            )
                            .ConfigureAwait(false);

                            galleryVideosCount += g.Value.Count;

                            string ids = "";
                            foreach (var item in g.Value)
                            {
                                ids += item.VideoId + ",";
                            }
                            ids = ids.Substring(0, ids.Length - 1);

                            p.id = ids;

                            var items = await Microsoft.Xbox.Ambassadors.API.YouTube.Videos.List<GalleryResponse>(p).ConfigureAwait(false);
                            items.NextPageToken = "";
                            foreach (var i in items.Items)
                            {
                                i.Value = g.Value.FirstOrDefault(x => x.VideoId == i.Id);
                            }
                            return items;
                        }
                        catch (Exception e) 
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                            return new GalleryResponse() { Items = Array.Empty<GalleryItem>() }; 
                        };
                    },
                    new VideosParameters() { part = "snippet,statistics" },
                    200
                );

                IsGalleryLoading = false;
            }
            catch (Exception e)
            {
                GalleryFailedAlertVisibility = Visibility.Visible;
                IsGalleryLoading = false;
            }
        }
        public async void Search(string query)
        {
            try
            {
                IsVideosLoading = true;
                VideosFailedAlertVisibility = Visibility.Collapsed;

                var r = await Microsoft.Xbox.Ambassadors.API.YouTube.Search.List(new SearchParameters()
                {
                    q = query,
                    safeSearch = SearchParameters.SafeSearch.strict,
                    type = "video",
                    order = SearchParameters.OrderType.date,
                    videoSyndicated = "true",
                    videoEmbeddable = "true",
                    videoDuration = "medium"
                });
                SearchItems = r.items;

                IsVideosLoading = false;

                App.AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("YouTube", "Search", query).Build());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                IsVideosLoading = false;
                VideosFailedAlertVisibility = Visibility.Visible;
            }
        }

        public async static Task<List<string>> FindSugestions(string query)
        {
            try
            {
                return await Microsoft.Xbox.Ambassadors.API.YouTube.Search.Suggest(query).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        private async void SubmitVideo()
        {
            try
            {
                SubmitVideoCommand.CanExecuteAction = false;

                var r = await Videos.PostVideoAsync(await AccessToken.LoadFromVaultOrGetNew(), SelectedItem.Id).ConfigureAwait(false);

                if (!r)
                {
                    NotificationService.Service.Notify("Video Gallery", "This video has been previously submitted or the channel is black listed.");
                }
                else
                {
                    NotificationService.Service.Notify("Video Gallery", "This video has been added and will be available in the gallery within a few minutes. ");
                }

                App.AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("YouTube", "Submit Video", SelectedItem.Id).Build());
            }
            catch
            {
                NotificationService.Service.Notify("Video Gallery", "An error occurred while processing the request.");
            }
        }

        private async void Vote()
        {
            try
            {
                DownVoteCommand.CanExecuteAction = false;

                var token = await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false);

                var r = await Videos.PostVoteAsync(token, SelectedItem.Id, "").ConfigureAwait(true);

                selectedItem.IsLiked = r;

                if (!r)
                {
                    NotificationService.Service.Notify("Video Gallery", "An error occurred while processing the request.");
                }

                App.AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("YouTube", "Vote", "Like").Build());
            }
            catch
            {
                NotificationService.Service.Notify("Video Gallery", "An error occurred while processing the request.");
            }
            finally
            {
                SelectedReason = null;
            }

        }
        private async void DownVote(string reason = "")
        {
            try
            {
                DownVoteCommand.CanExecuteAction = false;

                var token = await AccessToken.LoadFromVaultOrGetNew().ConfigureAwait(false);

                var r = await Videos.PostVoteAsync(token, SelectedItem.Id, reason).ConfigureAwait(true);

                selectedItem.IsDisliked = r;

                if (!r)
                {
                    NotificationService.Service.Notify("Video Gallery", "An error occurred while processing the request.");
                }

                App.AnalyticsTracker.Send(HitBuilder.CreateCustomEvent("YouTube", "Vote", "Dislike:" + reason).Build());
            }
            catch
            {
                NotificationService.Service.Notify("Video Gallery", "An error occurred while processing the request.");
            }
            finally
            {
                SelectedReason = null;
            }

        }

        public override void UpdateViewModel()
        {
            LoadGallery();
        }
    }

    [DataContract]
    public class GalleryResponse : Microsoft.Xbox.Ambassadors.API.YouTube.VideoResponseModels.VideosResponse<GalleryItem>
    {

    }
    public class GalleryItem : Microsoft.Xbox.Ambassadors.API.YouTube.VideoResponseModels.Item, INotifyPropertyChanged
    {
        bool isLiked = false;
        bool isDisliked = false;
        public Value Value { get; set; }
        public bool IsLiked
        {
            get => isLiked;
            set
            {
                isLiked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLiked)));
            }
        }

        public bool IsDisliked
        {
            get => isDisliked;
            set
            {
                isDisliked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDisliked)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static implicit operator GalleryItem(Microsoft.Xbox.Ambassadors.API.YouTube.SearchResponseModels.Item v)
        {
            return FromItem(v);
        }

        public static GalleryItem FromItem(Microsoft.Xbox.Ambassadors.API.YouTube.SearchResponseModels.Item v)
        {
            if (v == null) return null;

            GalleryItem item = new GalleryItem();
            item.ETag = v.ETag;
            item.Id = v.Id.VideoId;
            item.Kind = v.Kind;
            item.Snippet = new Microsoft.Xbox.Ambassadors.API.YouTube.VideoResponseModels.Snippet
            {
                ChannelId = v.Snippet.ChannelId,
                ChannelTitle = v.Snippet.ChannelTitle,
                Description = v.Snippet.Description,
                PublishedAt = v.Snippet.PublishedAt,
                LiveBroadcastContent = v.Snippet.LiveBroadcastContent,
                Thumbnails = new Microsoft.Xbox.Ambassadors.API.YouTube.Thumbnails
                {
                    Default = new Microsoft.Xbox.Ambassadors.API.YouTube.Thumbnail
                    {
                        Url = v.Snippet?.Thumbnails?.Default?.Url,
                    },
                    High = new Microsoft.Xbox.Ambassadors.API.YouTube.Thumbnail
                    {
                        Url = v.Snippet?.Thumbnails?.High?.Url
                    },
                    Medium = new Microsoft.Xbox.Ambassadors.API.YouTube.Thumbnail
                    {
                        Url = v.Snippet?.Thumbnails?.Medium?.Url
                    }
                },
                Title = v.Snippet.Title
            };

            return item;
        }
    }
}
