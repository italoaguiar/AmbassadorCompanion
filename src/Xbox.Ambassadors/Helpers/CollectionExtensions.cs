using Microsoft.Xbox.Ambassadors.API.Forum;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xbox.Ambassadors.IncrementalLoading;

namespace Xbox.Ambassadors.Helpers
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
        {
            return new ObservableCollection<T>(list);
        }
        public static void UpdateCollection(this ForumLoader<ForumThread> collection, IList<ForumThread> newCollection)
        {
            if ((null == newCollection) || (newCollection.Count == 0))
            {
                collection.Clear();
                return;
            }

            var i = 0;
            foreach (var it in newCollection)
            {
                if (collection.Count > i)
                {
                    var itemIndex = collection.IndexOf(collection.FirstOrDefault(x => ((ForumThread)x).Id == it.Id));

                    if (itemIndex < 0)
                    {
                        collection.InsertAt(i, it);
                    }
                    else if (itemIndex > i)
                    {
                        collection.MoveTo(itemIndex, i);
                    }
                    else
                    {
                        if (((ForumThread)collection[i]).Id != it.Id)
                        {
                            collection.InsertAt(i, it);
                        }
                        else
                        {
                            var f = (ForumThread)collection[i];
                            f.Author = it.Author;
                            f.Date = it.Date;
                            f.Description = it.Description;
                            f.HtmlDescription = it.HtmlDescription;
                            f.IsLocked = it.IsLocked;
                            f.Language = it.Language;
                            f.LastReply = it.LastReply;
                            f.LastReplyAuthor = it.LastReplyAuthor;
                            f.Posts = it.Posts;
                            f.ShortDescription = it.ShortDescription;
                            f.Title = it.Title;
                            f.TotalHelpful = it.TotalHelpful;
                            f.TotalNeedAnswer = it.TotalNeedAnswer;
                            f.TotalReplies = it.TotalReplies;
                            f.TotalViews = it.TotalViews;

                        }
                    }
                }
                else
                {
                    collection.AddItem(it);
                }


                i++;
            }
            //while (collection.Count > newCollection.Count)
            //{
            //    collection.RemoveAt(i);
            //};
        }
        public static IEnumerable<T> Move<T>(this IList<T> collection, int oldIndex, int newIndex)
        {
            T item = collection[oldIndex];
            collection.RemoveAt(oldIndex);
            collection.Insert(newIndex, item);

            return collection;
        }
    }

}
