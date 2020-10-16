using Microsoft.Xbox.Ambassadors.API.Forum;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Xbox.Ambassadors.IncrementalLoading
{
    public class ForumLoader<T> : List<T>, ISupportIncrementalLoading, INotifyCollectionChanged where T : ForumThread
    {
        public ForumLoader(int max_page = 6)
        {
            maxPage = max_page;
            forum = new Forum();
        }

        private int page = 1;
        private int maxPage = 6;
        private Forum forum;
        bool _busy = false;

        protected bool HasMoreItemsOverride()
        {
            return page < maxPage;
        }
        protected async Task<LoadMoreItemsResult> LoadMoreItemsOverrideAsync(CancellationToken c, uint count)
        {
            try
            {
                var r = await forum.GetThreadsAsync(page);
                page++;

                AddItemRange((IList<T>)r);

                return new LoadMoreItemsResult() { Count = (uint)r.Count };
            }
            catch (Exception e)
            {
                var k = e;
                return new LoadMoreItemsResult();
            }

        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            _busy = true;

            return AsyncInfo.Run((c) => LoadMoreItemsOverrideAsync(c, count));
        }



        public void AddItem(T item)
        {
            this.Add(item);

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, Count - 1);
            OnCollectionChanged(args);
        }

        public void RemoveItemAt(int index)
        {
            T item = this[index];
            this.RemoveAt(index);

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
            OnCollectionChanged(args);
        }


        public void AddItemRange(IList<T> items)
        {
            foreach (var i in items) AddItem(i);
        }

        public void MoveTo(int oldPosition, int newPosition)
        {
            T item = base[oldPosition];
            RemoveAt(oldPosition);
            InsertAt(newPosition, item);
        }

        public void InsertAt(int index, T item)
        {
            this.Insert(index, item);
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
            OnCollectionChanged(args);
        }


        public void ClearAll()
        {
            this.Clear();
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(args);
        }





        public bool HasMoreItems => HasMoreItemsOverride();


        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
