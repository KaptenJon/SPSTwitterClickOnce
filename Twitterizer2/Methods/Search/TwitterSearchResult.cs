﻿//-----------------------------------------------------------------------
// <copyright file="TwitterSearchResult.cs" company="Patrick 'Ricky' Smith">
//  This file is part of the TwitterConnector library (http://www.TwitterConnector.net/)
// 
//  Copyright (c) 2010, Patrick "Ricky" Smith (ricky@digitally-born.com)
//  All rights reserved.
//  
//  Redistribution and use in source and binary forms, with or without modification, are 
//  permitted provided that the following conditions are met:
// 
//  - Redistributions of source code must retain the above copyright notice, this list 
//    of conditions and the following disclaimer.
//  - Redistributions in binary form must reproduce the above copyright notice, this list 
//    of conditions and the following disclaimer in the documentation and/or other 
//    materials provided with the distribution.
//  - Neither the name of the TwitterConnector nor the names of its contributors may be 
//    used to endorse or promote products derived from this software without specific 
//    prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//  POSSIBILITY OF SUCH DAMAGE.
// </copyright>
// <author>Ricky Smith</author>
// <summary>The twitter search result class.</summary>
//-----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using TwitterConnector.Core;
using TwitterConnector.Entities;

namespace TwitterConnector
{
    /// <summary>
    ///     The Twitter Search Result class.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TwitterSearchResult : TwitterObject
    {
        /// <summary>
        ///     Gets or sets the user.
        /// </summary>
        /// <value>The user that posted this status.</value>
        [DataMember, JsonProperty(PropertyName = "user")]
        public TwitterUser User { get; set; }

        /// <summary>
        ///     Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        [JsonProperty(PropertyName = "created_at")]
        [JsonConverter(typeof (TwitterConnectorDateConverter))]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///     Gets or sets the status text.
        /// </summary>
        /// <value>The status text.</value>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        ///     Returns the status text with HTML links to users, urls, and hashtags.
        /// </summary>
        /// <remarks>
        ///     This will only work if you specify <see cref="SearchOptions.IncludeEntities" /> = <c>true</c> when executing
        ///     the search.
        /// </remarks>
        /// <returns></returns>
        public string LinkifiedText()
        {
            return TwitterStatus.LinkifiedText(Entities, Text);
        }

        /// <summary>
        ///     Gets or sets the status id.
        /// </summary>
        /// <value>The status id.</value>
        [JsonProperty(PropertyName = "id_str")]
        public decimal Id { get; set; }

        /// <summary>
        ///     Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        /// <summary>
        ///     Gets or sets the geo location data.
        /// </summary>
        /// <value>The geo location data.</value>
        [DataMember, JsonProperty(PropertyName = "geo")]
        public TwitterGeo Geo { get; set; }

        /// <summary>
        ///     Gets or sets the status id the status is in reply to.
        /// </summary>
        /// <value>The status id.</value>
        [DataMember, JsonProperty(PropertyName = "in_reply_to_status_id")]
        public decimal? InReplyToStatusId { get; set; }

        /// <summary>
        ///     Gets or sets the favorite count string.
        /// </summary>
        /// <value>The favorite count.</value>
        [JsonProperty(PropertyName = "favorite_count")]
        public int? FavoriteCount { get; set; }

        /// <summary>
        ///     Gets or sets the retweet count string.
        /// </summary>
        /// <value>The retweet count.</value>
        [DataMember, JsonProperty(PropertyName = "retweet_count")]
        public string RetweetCountString { get; set; }

        /// <summary>
        ///     Gets the retweet count.
        /// </summary>
        /// <value>The retweet count.</value>
        [DataMember]
        public int? RetweetCount
        {
            get
            {
                if (string.IsNullOrEmpty(RetweetCountString)) return null;

                int parsedResult;

                if (
                    RetweetCountString.EndsWith("+") &&
                    !int.TryParse(RetweetCountString.Substring(0, RetweetCountString.Length - 1), out parsedResult)
                    )
                {
                    return null;
                }

                if (!int.TryParse(RetweetCountString, out parsedResult))
                {
                    return null;
                }

                return parsedResult;
            }
        }

        /// <summary>
        ///     Gets a value indicating that the number of retweets exceeds the reported value in RetweetCount. For example, "more
        ///     than 100"
        /// </summary>
        /// <value>The retweet count plus indicator.</value>
        [DataMember]
        public bool? RetweetCountPlus
        {
            get
            {
                if (string.IsNullOrEmpty(RetweetCountString)) return null;

                return RetweetCountString.EndsWith("+");
            }
        }

        /// <summary>
        ///     Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        [DataMember]
        [JsonProperty(PropertyName = "entities")]
        [JsonConverter(typeof (TwitterEntityCollection.Converter))]
        public TwitterEntityCollection Entities { get; set; }
    }
}