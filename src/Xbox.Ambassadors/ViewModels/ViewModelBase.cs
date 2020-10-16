using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xbox.Ambassadors.Services;

namespace Xbox.Ambassadors.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        public NavigationService NavigationService { get; set; } = new NavigationService();

        public static Frame ContentFrame { get; set; }

        protected ViewModelBase(FrameworkElement e)
        {
            Dispatcher = e.Dispatcher;
        }

        protected readonly CoreDispatcher Dispatcher;



        internal ViewModelBase RegisterTimeTrigger(Action f, TimeSpan ts)
        {
            Actions.Add(new UpdateElement()
            {
                Action = f,
                UpdateInterval = ts,
                LastUpdate = DateTime.MinValue
            });
            return this;
        }


        List<UpdateElement> Actions = new List<UpdateElement>();


        private async void UpdateOutdatedSections()
        {
            if (NetworkInformation.GetInternetConnectionProfile() != null)
                foreach (var a in Actions)
                {
                    if (a.IsOutdated)
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => a.Action());
                        a.LastUpdate = DateTime.Now;
                    }
                }
        }

        public void Tick()
        {
            UpdateOutdatedSections();
        }

        public abstract void UpdateViewModel();



        public event PropertyChangedEventHandler PropertyChanged;




        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private class UpdateElement
        {
            public Action Action { get; set; }
            public TimeSpan UpdateInterval { get; set; }
            public DateTime LastUpdate { get; set; }

            public bool IsOutdated
            {
                get
                {
                    return (LastUpdate + UpdateInterval) < DateTime.Now;
                }
            }
        }
    }
}
