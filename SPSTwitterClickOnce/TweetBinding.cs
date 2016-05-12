using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using SPSTwitterClickOnce.Annotations;
using SPSTwitterClickOnce.TweetHost;

namespace SPSTwitterClickOnce
{
    public class TweetBinding : ViewModelBase
    {
       
        private ObservableCollection<ListViewBindingObject> _resultlist = null;
        private DispatcherTimer timer = new DispatcherTimer();
        private static TweetHost.Service1Client host;

        public TweetBinding()
        {
            _nrOfTweets = 8;
            if (ViewModelBase.IsInDesignModeStatic)
            {
                Resultlist = new ObservableCollection<ListViewBindingObject>(ListViewBindingObjects.Take(_nrOfTweets));
                return;
            }

            host = new TweetHost.Service1Client();
            
            Resultlist = new ObservableCollection<ListViewBindingObject>();
            UpdateQuery();

            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
         }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            UpdateQuery();
            timer.Start();
        }

        private async void UpdateQuery()
        {
            
            try
            {
                var t = host.GetTweetsAsync();
                var res = await t;
                UpdateList(res);
            }
            catch (Exception)
            {
            }
        }

        private void UpdateList(IEnumerable<ListViewBindingObject> take)
        {
            var twitterSearchResults = take as ListViewBindingObject[] ?? take.ToArray();
            var newit = twitterSearchResults.Count();
            newit = newit > _nrOfTweets ? _nrOfTweets : newit;
            var old = Resultlist.Count;
            var remove = old - _nrOfTweets + newit;
            while (remove > 0)
            {
                remove--;
                Resultlist.RemoveAt(Resultlist.Count - 1);
            }
            foreach (var twitterSearchResult in twitterSearchResults)
            {
                if (Resultlist.All(t => t.Message != twitterSearchResult.Message))
                {
                    Resultlist.Add(twitterSearchResult);
                    if (Resultlist.Count >= _nrOfTweets)
                        break;
                }
            }
        }

        private ObservableCollection<ListViewBindingObject> _resultlistFirstHalf = new ObservableCollection<ListViewBindingObject>();
        public ObservableCollection<ListViewBindingObject> ResultlistFirstHalf
        {
            get { return _resultlistFirstHalf; }
        }
        private ObservableCollection<ListViewBindingObject> _resultlistSecondHalf = new ObservableCollection<ListViewBindingObject>();
        public ObservableCollection<ListViewBindingObject> ResultlistSecondHalf
        {
            get { return _resultlistSecondHalf; }
        }

        public ObservableCollection<ListViewBindingObject> Resultlist
        {
            get { return _resultlist; }
            set
            {
                if (Equals(value, _resultlist)) return;
                _resultlist = value;
                _resultlist.CollectionChanged += ResultlistOnCollectionChanged;
                ResultlistOnCollectionChanged(this,new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                OnPropertyChanged();
            }
        }

        private void ResultlistOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            
                int half = _resultlist.Count/2;
                _resultlistFirstHalf.Clear();
                _resultlistSecondHalf.Clear();
                foreach (var bindingObject in Resultlist.Take(half))
                {
                    _resultlistFirstHalf.Add(bindingObject);
                }
                foreach (var bindingObject in Resultlist.Skip(half))
                {
                    _resultlistSecondHalf.Add(bindingObject);   
                }
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Design Time
        /// </summary>
        private static readonly ObservableCollection<ListViewBindingObject> ListViewBindingObjects = new ObservableCollection
            <ListViewBindingObject>()
        {
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                ContentImage = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png"
            }
        };

        private static int _nrOfTweets;
    }
}
    

