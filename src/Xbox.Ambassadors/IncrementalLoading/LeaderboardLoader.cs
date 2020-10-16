using Microsoft.Xbox.Ambassadors.API;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.IncrementalLoading
{
    public class LeaderboardLoader : List<Item>, ISupportIncrementalLoading, INotifyCollectionChanged
    {
        public LeaderboardLoader(long seasonId)
        {
            this.seasonId = seasonId;
        }

        private long seasonId;
        private long totalCount = 1;
        private long actualCount;
        private uint page = 0;


        private async Task<LoadMoreItemsResult> LoadMoreItemsOverrideAsync(CancellationToken c, uint count)
        {
            try
            {
                var r = await Leaderboard.GetAsync(await AccessToken.LoadFromVaultOrGetNew(), (uint)seasonId, 50, page);
                totalCount = r.TotalCount;
                actualCount += r.Items.Count();
                AddItemRange(r.Items);
                page++;
                return new LoadMoreItemsResult() { Count = (uint)r.Items.Count };
            }
            catch
            {
                return new LoadMoreItemsResult();
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run((c) => LoadMoreItemsOverrideAsync(c, count));
        }

        public bool HasMoreItems
        {
            get
            {
                return actualCount < totalCount;
            }
        }

        public void AddItem(Item item)
        {
            this.Add(item);

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count - 1);
            OnCollectionChanged(args);
        }

        public void AddItemRange(IList<Item> items)
        {
            foreach (var i in items) AddItem(i);
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
