using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using SPSTwitterStore.TweetHost;

namespace SPSTwitterStore
{
    public class TweetBinding : INotifyPropertyChanged
    {
       
        private ObservableCollection<ListViewBindingObject> _resultlist = null;
        private DispatcherTimer timer = new DispatcherTimer();
        private static TweetHost.Service1Client host;

        public TweetBinding()
        {
            _nrOfTweets = 8;
            if (false)
            {
                Resultlist = new ObservableCollection<ListViewBindingObject>(ListViewBindingObjects.Take(_nrOfTweets));
                UpdateList(Resultlist);
                return;
            }

            host = new TweetHost.Service1Client();
            
            Resultlist = new ObservableCollection<ListViewBindingObject>();
            UpdateQuery();

            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += TimerOnTick;   
            timer.Start();
         }

        private void TimerOnTick(object sender, object o)
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
            var twitterSearchResults = take as List<ListViewBindingObject> ?? take.ToList();
            twitterSearchResults.RemoveAll(t => Resultlist.Any(y => y.Message == t.Message));

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
                
            },
            new ListViewBindingObject()
            {
                Messenger = @"https://abs.twimg.com/sticky/default_profile_images/default_profile_5_bigger.png",
                Message =
                    "Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Testar ett fullt medelande Klar!",
                
            }
        };

        private static int _nrOfTweets;


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
    

