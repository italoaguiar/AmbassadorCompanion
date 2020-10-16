//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DataBinding
{
    // This class can used as a jumpstart for implementing ISupportIncrementalLoading. 
    // Implementing the ISupportIncrementalLoading interfaces allows you to create a list that loads
    //  more data automatically when the user scrolls to the end of of a GridView or ListView.
    public abstract class IncrementalLoadingBase : ObservableCollection<object>, ISupportIncrementalLoading, INotifyCollectionChanged, INotifyPropertyChanged
    {


        #region ISupportIncrementalLoading

        public bool HasMoreItems
        {
            get { return HasMoreItemsOverride(); }
        }

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (_busy)
            {
                throw new InvalidOperationException("Only one operation in flight at a time");
            }

            _busy = true;
            OnPropertyChanged("IsLoading");

            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        #endregion

        #region INotifyCollectionChanged

        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Private methods

        async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            try
            {
                IsLoading = true;
                var items = await LoadMoreItemsOverrideAsync(c, count);
                var baseIndex = this.Count;

                //_storage.AddRange(items);

                foreach (var i in items) this.Add(i);

                // Now notify of the new items
                NotifyOfInsertedItems(baseIndex, items.Count);

                return new LoadMoreItemsResult { Count = (uint)items.Count };
            }
            finally
            {
                _busy = false;
                IsLoading = false;
            }
        }

        public LoadMoreItemsResult InsertAt(object value, int index)
        {
            this.Insert(index, value);

            NotifyOfInsertedItems(index, 1);

            return new LoadMoreItemsResult { Count = 1 };
        }

        public void MoveOverride(int oldIndex, int newIndex)
        {
            this.Move(oldIndex, newIndex);

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, this[newIndex], newIndex, oldIndex);
            CollectionChanged(this, args);
        }

        public void AddOverride(object item)
        {
            this.Add(item);

            NotifyOfInsertedItems(Count - 1, 1);
        }

        public void RemoveAtOverride(int index)
        {
            var removed = this[index];
            this.RemoveAt(index);

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed);
            CollectionChanged(this, args);
        }

        void NotifyOfInsertedItems(int baseIndex, int count)
        {
            if (CollectionChanged == null)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this[i + baseIndex], i + baseIndex);
                CollectionChanged(this, args);
            }
        }

        #endregion

        #region Overridable methods

        protected abstract Task<ObservableCollection<object>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count);
        protected abstract bool HasMoreItemsOverride();

        #endregion

        #region State


        bool _busy = false;
        bool _isLoading = false;

        public bool CanLoadMoreItems
        {
            get
            {
                return !_busy;
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        #endregion
    }
}
