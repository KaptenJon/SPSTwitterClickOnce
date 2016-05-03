//-----------------------------------------------------------------------
// <copyright file="UpdateFriendshipCommand.cs" company="Patrick 'Ricky' Smith">
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
// <summary>The Direct Messages Sent Command class.</summary>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using TwitterConnector.Core;

namespace TwitterConnector.Commands
{
    /// <summary>
    ///     Creates a friendship between the authenticated user and another user
    /// </summary>
    [AuthorizedCommand]
#if !SILVERLIGHT
    [Serializable]
#endif
    internal sealed class UpdateFriendshipCommand : TwitterCommand<TwitterRelationship>
    {
        /// <summary>
        ///     The base address to the API method.
        /// </summary>
        private const string Path = "friendships/update.json";

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateFriendshipCommand" /> class.
        /// </summary>
        /// <param name="tokens">The request tokens.</param>
        /// <param name="userId">The userid.</param>
        /// <param name="optionalProperties">The optional properties.</param>
        public UpdateFriendshipCommand(OAuthTokens tokens, decimal userId, UpdateFriendshipOptions optionalProperties)
            : base(HTTPVerb.POST, Path, tokens, optionalProperties)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }

            if (userId <= 0)
            {
                throw new ArgumentException("userId");
            }

            UserId = userId;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateFriendshipCommand" /> class.
        /// </summary>
        /// <param name="tokens">The request tokens.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="optionalProperties">The optional properties.</param>
        public UpdateFriendshipCommand(OAuthTokens tokens, string userName, UpdateFriendshipOptions optionalProperties)
            : base(HTTPVerb.POST, Path, tokens, optionalProperties)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }

            UserName = userName;
        }

        /// <summary>
        ///     Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public decimal UserId { get; set; }

        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string UserName { get; set; }

        /// <summary>
        ///     Initializes the command.
        /// </summary>
        public override void Init()
        {
            if (UserId > 0)
            {
                RequestParameters.Add("user_id", UserId.ToString(CultureInfo.InvariantCulture));
            }
            else if (!string.IsNullOrEmpty(UserName))
            {
                RequestParameters.Add("screen_name", UserName);
            }

            var options = OptionalProperties as UpdateFriendshipOptions;
            if (options != null)
            {
                if (options.DeviceNotifications != null)
                    RequestParameters.Add("device", (bool) options.DeviceNotifications ? "true" : "false");

                if (options.ShowRetweets != null)
                    RequestParameters.Add("retweets", (bool) options.ShowRetweets ? "true" : "false");
            }
        }
    }
}