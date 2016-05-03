using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using InstaSharp.Models;
using TwitterConnector.Annotations;
using TwitterConnector.Entities;

namespace TwitterConnector
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class ListViewBindingObject : INotifyPropertyChanged
    {
        private string _contentImage;
        private string _message;
        private string _messenger;
        

        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        public ListViewBindingObject(TwitterSearchResult result)
        {
            Message = result.Text;
            Messenger = result.User.ProfileImageSecureLocation;
            var media = (TwitterMediaEntity) result.Entities.FirstOrDefault(t => t is TwitterMediaEntity);
            if (media != null) ContentImage = media.MediaUrlSecure;
            CreatedTime = result.CreatedDate;
        }

        /// <summary>
        /// manual create
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="message"></param>
        /// <param name="contentImage"></param>
        /// <param name="creatTime"></param>
        public ListViewBindingObject(string messenger, string message, string contentImage, DateTime creatTime)
        {
            Messenger = messenger;
            Message = message;
            ContentImage = contentImage;
            CreatedTime = creatTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messenger"></param>
        /// <exception cref="NotImplementedException"></exception>
        public ListViewBindingObject(Media messenger)
        {
            Messenger = messenger.User.ProfilePicture;
            Message = messenger.Caption.Text;
            ContentImage = messenger.Images.StandardResolution.Url;
            CreatedTime = messenger.CreatedTime;
        }


        /// <summary>
        ///     One of the linked images
        /// </summary>
        [DataMember]
        public string ContentImage
        {
            get { return _contentImage; }
            set
            {
                if (value == _contentImage) return;
                _contentImage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sorting date
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///     Messengers Image
        /// </summary>
        [DataMember]
        public string Messenger
        {
            get { return _messenger; }
             set
            {
                if (value == _messenger) return;
                _messenger = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Message To Display
        /// </summary>
        [DataMember]
        public string Message
        {
            get { return _message; }
             set
            {
                if (value == _message) return;
                _message = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        ///     Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Property changed handler
        /// </summary>
        /// <param name="propertyName">Property that has changed</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ListViewBindingObjectComparer : IComparer<ListViewBindingObject>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(ListViewBindingObject x, ListViewBindingObject y)
        {
            return y.CreatedTime.CompareTo(x.CreatedTime);
        }
        
    }
}