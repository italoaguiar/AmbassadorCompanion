using Microsoft.Xbox.Ambassadors.API.Forum;
using System.ComponentModel;
using System.Threading.Tasks;
using Xbox.Ambassadors.Helpers;
using Xbox.Ambassadors.IncrementalLoading;

namespace Xbox.Ambassadors.Services
{
    public class ForumService : INotifyPropertyChanged
    {
        private ForumService() { }

        //singleton
        public static ForumService Service { get; set; } = new ForumService();

        public async Task<ForumLoader<ForumThread>> UpdateAsync()
        {
            Forum f = new Forum();
            var r = await f.GetThreadsAsync();

            Threads.UpdateCollection(r);

            //TopThreads = Threads.Where(x => !x.IsLocked).Take(8).ToList();

            return Threads;
        }

        ForumLoader<ForumThread> _threads = new ForumLoader<ForumThread>();

        public ForumLoader<ForumThread> Threads
        {
            get { return _threads; }
            set
            {
                _threads = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Threads"));
            }
        }

        //private List<ForumThread> _topThreads;
        //public List<ForumThread> TopThreads
        //{
        //    get
        //    {
        //        return _topThreads;
        //    }
        //    private set
        //    {
        //        _topThreads = value;

        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TopThreads"));
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;


    }

}
