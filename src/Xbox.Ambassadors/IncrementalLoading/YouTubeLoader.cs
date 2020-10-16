using DataBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.ObjectModel;
using Microsoft.Xbox.Ambassadors.API.YouTube;
using Microsoft.Xbox.Ambassadors.API.YouTube.Parameters;
using Microsoft.Xbox.Ambassadors.API.Auth;

namespace YouTube.API.Util
{
    public class YouTubeIncrementalLoader<T1, T2> : YouTubeIncrementalLoadingBase
        where T1 : Response
        where T2 : Parameter
    {
        protected bool _autoIncrement = true;
        public bool AutoIncrement
        {
            get { return _autoIncrement; }
            set { _autoIncrement = value; }
        }


        private Func<T2, Task<T1>> func;
        private Func<AccessToken, T2, Task<T1>> authFunc;
        private T2 parameter;
        private string pageToken = string.Empty;
        private int MaxResults = 120, CurrentLoad = 0;
        private bool useToken = false;
        private AccessToken token;


        public YouTubeIncrementalLoader(Func<AccessToken,T2, Task<T1>> r, T2 p, AccessToken _token)
        {
            authFunc = r;
            parameter = p;
            token = _token;
            useToken = _token != null && r != null;
        }
        public YouTubeIncrementalLoader(Func<T2, Task<T1>> r, T2 p)
        {
            func = r;
            parameter = p;
        }
        public YouTubeIncrementalLoader(Func<T2, Task<T1>> r, T2 p, int MaxResults) : this(r, p)
        {
            this.MaxResults = MaxResults;
        }
        public YouTubeIncrementalLoader(Func<AccessToken, T2, Task<T1>> r, T2 p, AccessToken token, int MaxResults) : this(r, p, token)
        {
            this.MaxResults = MaxResults;
        }
        protected override bool HasMoreItemsOverride()
        {
            return pageToken != null && CurrentLoad < MaxResults;
        }

        protected async override Task<List<object>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count)
        {
            List<object> items = new List<object>();
            if (!string.IsNullOrEmpty(pageToken)) parameter.pageToken = pageToken;
            var t = useToken ? await authFunc(token,parameter): await func(parameter);
            pageToken = t.NextPageToken;
            CurrentLoad += t.GetItems().Count();
            items.AddRange(t.GetItems());
            return items;
        }
    }
    public static class LinqExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
        {
            return new ObservableCollection<T>(items);
        }
        public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var i in items) collection.Add(i);
            return collection;
        }
    }

}
