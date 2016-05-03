using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.EnterpriseServices;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InstaSharp;
using InstaSharp.Models;
using InstaSharp.Models.Responses;
using TwitterConnector;
using Timer = System.Timers.Timer;

namespace TwitterHost
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private static  OAuthTokens _tokens=null;
        private static string _hashtag = "#SPS2014";
        private static string _InstagramClentID = "98fd8f8a89d548c38299c7c6a8fe2e21";
        private static SearchOptions _searchOptions;
        private static int _nrOfTweets = 8;
        private static float _mintag;
        private static DateTime _sinceDate = DateTime.MinValue;
        private static string _hashtag2 =  "#SPS14";
        private static float _mintag2 = 0;
        private static string _hashtagcurrent = "";
        
        public Service1()
        {
            try
            {
                _hashtag = ConfigurationManager.AppSettings["hashtag"];
                _hashtag2 = ConfigurationManager.AppSettings["hashtag2"];
            }
            catch { }
            try{_nrOfTweets = int.Parse(ConfigurationManager.AppSettings["nrOfTweets"]);}
            catch { }
            if (_tokens == null)
            {
                //Twitter
                _tokens = new OAuthTokens();
                _tokens.ConsumerKey = "wiOXH5Ayw0XCOrN3f5kQuhBC6"; //<-- replace with yours
                _tokens.ConsumerSecret = "vuQfuqr4wxygnicNiVDTP4WscTtpM68ivsKibjsxXylDFNQv5Q"; //<-- replace with yours
                _tokens.AccessToken = "761173362-78G2mbA8qvJn4AhUkkpFYWTqWczBi14miqh6Biyb"; //<-- replace with yours
                _tokens.AccessTokenSecret = "Nx1daFg6rqJBFaGduW5I7TLLuKv1LASsJgz6p31zm4BIB"; //<-- replace with yours



                _searchOptions = new SearchOptions();
                _searchOptions.UseSSL = true;
                _searchOptions.ResultType = SearchOptionsResultType.Recent;

                //Instagram
                //_instagramConfig = new InstagramConfig()
                
                //new InstagramConfig(_InstagramClentID, _InstagramClientSecret);
                //_instagramConfig.RedirectUri = _instagramredirect;
                //_instagramOAuth = new OAuth(_instagramConfig);
                //var t = _instagramOAuth.RequestToken("180639561.98fd8f8.c88d7058d2644902a4e834b08ec074ae");
                //t.Wait();
                //_instagramResponce = t.Result;
                //_instagramResponce = new OAuthResponse() { Access_Token = _instagramauth };
                Resultlist = new List<ListViewBindingObject>();
                UpdateQuery();
            }
            //if (timer == null || timer.Enabled == false)
            //{
            //    timer = new Timer(60);
            //    timer.Elapsed += timer_Elapsed;
            //    timer.Start();
            //}
        }



        private static List<ListViewBindingObject> Resultlist { get; set; }
                   
        private async static void UpdateQuery()
        {
            _sinceDate = DateTime.Now;
            var locald = _sinceDate;
            _hashtagcurrent = _hashtag == _hashtagcurrent ? _hashtag2 : _hashtag;
            var usedmintag = _hashtag == _hashtagcurrent ? _mintag : _mintag2;  
            try
            {
                TwitterResponse<TwitterSearchResultCollection> timeline = TwitterSearch.Search(_tokens, _hashtagcurrent, _searchOptions);
                if (!timeline.Content.StartsWith("{\"errors"))
                    AddList(timeline.ResponseObject);

                var tags = new InstaSharp.Endpoints.Tags(_InstagramClentID);
                var media = await tags.Recent(_hashtagcurrent.Replace("#", ""), usedmintag.ToString(CultureInfo.InvariantCulture), "", _nrOfTweets);
                media.Data.RemoveAll(t => t.CreatedTime < _searchOptions.SinceDate);
                AddList(media.Data);
                SortAndRemove();

                
                 _searchOptions.SinceDate = locald - TimeSpan.FromMinutes(1);
                try
                {
                    if(_hashtagcurrent == _hashtag)
                        _mintag = media.Data.Max(t => float.Parse(t.Id));
                    else
                    {
                        _mintag2 = media.Data.Max(t => float.Parse(t.Id));
                    }
                }
                catch
                {
                }
            }
            catch (Exception)
            {
            }

            
        }

        private static void AddList(List<Media> data)
        {
            var list = data.ToArray();
            foreach (var media in list)
            {
                if (!Resultlist.Any(t => t.Message == media.Caption.Text))
                    Resultlist.Add(new ListViewBindingObject(media));
            }
            
        }

        private static void AddList(IEnumerable<TwitterSearchResult> take)
        {
            var twitterSearchResults = take as TwitterSearchResult[] ?? take.ToArray();

            foreach (var twitterSearchResult in twitterSearchResults)
            {
                if (!Resultlist.Any(t => t.Message == twitterSearchResult.Text))
                    Resultlist.Add(new ListViewBindingObject(twitterSearchResult));
            }
        }

        private static void SortAndRemove()
        {
            Resultlist.Sort(new ListViewBindingObjectComparer());
            if(Resultlist.Count > 8)
                Resultlist.RemoveRange(8,Resultlist.Count()-8);
        }

        public async Task<ListViewBindingObject[]> GetTweets()
        {
            var myTask = Task.Factory.StartNew(() =>
            {
                if (_sinceDate < DateTime.Now - TimeSpan.FromMinutes(1))
                    UpdateQuery();
            });
            await myTask;
            return Resultlist.ToArray();
        }
    }
}
