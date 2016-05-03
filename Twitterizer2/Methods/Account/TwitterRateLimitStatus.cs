//-----------------------------------------------------------------------
// <copyright file="TwitterRateLimitStatus.cs" company="Patrick 'Ricky' Smith">
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
// <summary>The twitter rate limit status class.</summary>
//-----------------------------------------------------------------------

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwitterConnector.Commands;
using TwitterConnector.Core;
using TwitterConnector.RateLimitStatus;

namespace TwitterConnector
{
    /// <summary>
    ///     The Twitter Rate Limit Status class
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TwitterRateLimitStatus : TwitterObject
    {
        #region API

        /// <summary>
        ///     Gets or sets the account of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "account")]
        public RateLimitAccount Account { get; set; }

        /// <summary>
        ///     Gets or sets the application of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "application")]
        public RateLimitApplication Application { get; set; }

        /// <summary>
        ///     Gets or sets the blocks of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "blocks")]
        public RateLimitBlock Blocks { get; set; }

        /// <summary>
        ///     Gets or sets the DMs of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "direct_messages")]
        public RateLimitDirectMessage DirectMessages { get; set; }

        /// <summary>
        ///     Gets or sets the favorites of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "favorites")]
        public RateLimitFavorites Favorites { get; set; }

        /// <summary>
        ///     Gets or sets the followers of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "followers")]
        public RateLimitFollowers Followers { get; set; }

        /// <summary>
        ///     Gets or sets the friends of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "friends")]
        public RateLimitFriends Friends { get; set; }

        /// <summary>
        ///     Gets or sets the friendships of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "friendships")]
        public RateLimitFriendShips FriendShips { get; set; }

        /// <summary>
        ///     Gets or sets the geo of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "geo")]
        public RateLimitGeo Geo { get; set; }

        /// <summary>
        ///     Gets or sets the lists of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "lists")]
        public RateLimitLists Lists { get; set; }

        /// <summary>
        ///     Gets or sets the savedsearches of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "saved_searches")]
        public RateLimitSavedSearches SavedSearches { get; set; }

        /// <summary>
        ///     Gets or sets the search of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "search")]
        public RateLimitSearch Search { get; set; }

        /// <summary>
        ///     Gets or sets the statuses of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "statuses")]
        public RateLimitStatuses Statuses { get; set; }

        /// <summary>
        ///     Gets or set the trends of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "trends")]
        public RateLimitTrends Trends { get; set; }

        /// <summary>
        ///     Gets or sets the users of rate-limit.
        /// </summary>
        [JsonProperty(PropertyName = "users")]
        public RateLimitUsers Users { get; set; }

        #endregion

        /// <summary>
        ///     Gets the rate limiting status status for the authenticated user.
        /// </summary>
        /// <param name="tokens">The OAuth tokens.</param>
        /// <param name="options">The options.</param>
        /// <param name="resource">The resources.</param>
        /// <returns>
        ///     A <see cref="TwitterRateLimitStatus" /> instance.
        /// </returns>
        public static TwitterResponse<TwitterRateLimitStatus> GetStatus(OAuthTokens tokens, string resource,
            OptionalProperties options)
        {
            var command = new RateLimitStatusCommand(tokens, resource, options);
            TwitterResponse<TwitterRateLimitStatus> result = CommandPerformer.PerformAction(command);

            return result;
        }

        /// <summary>
        ///     Gets the rate limiting status status based on the application's IP address.
        /// </summary>
        /// <param name="tokens">The OAuth tokens.</param>
        /// <returns>
        ///     A <see cref="TwitterRateLimitStatus" /> instance.
        /// </returns>
        public static TwitterResponse<TwitterRateLimitStatus> GetStatus(OAuthTokens tokens)
        {
            return GetStatus(tokens, null, null);
        }


        internal static TwitterRateLimitStatus Deserialize(JObject value)
        {
            if (value == null || value["resources"] == null)
                return null;

            var result = JsonConvert.DeserializeObject<TwitterRateLimitStatus>(value["resources"].ToString());

            return result;
        }
    }
}